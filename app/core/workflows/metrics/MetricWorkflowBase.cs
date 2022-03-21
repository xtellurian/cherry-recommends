using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Metrics.Destinations;

namespace SignalBox.Core.Workflows
{
    public abstract class MetricWorkflowBase
    {
        protected readonly IMetricStore metricStore;
        protected readonly IHistoricCustomerMetricStore customerMetricStore;
        private readonly IRecommenderMetricTriggersWorkflow triggersWorkflows;
        private readonly HubspotPushWorkflows hubspotPushWorkflows;
        private readonly IWebhookSenderClient webhookSenderClient;
        private readonly ITelemetry telemetry;
        protected readonly ILogger<MetricWorkflowBase> logger;

        protected MetricWorkflowBase(IMetricStore metricStore,
                                   IHistoricCustomerMetricStore customerMetricStore,
                                   IRecommenderMetricTriggersWorkflow triggersWorkflows,
                                   HubspotPushWorkflows hubspotPushWorkflows,
                                   IWebhookSenderClient webhookSenderClient,
                                   ITelemetry telemetry,
                                   ILogger<MetricWorkflowBase> logger)
        {
            this.metricStore = metricStore;
            this.customerMetricStore = customerMetricStore;
            this.triggersWorkflows = triggersWorkflows;
            this.hubspotPushWorkflows = hubspotPushWorkflows;
            this.webhookSenderClient = webhookSenderClient;
            this.telemetry = telemetry;
            this.logger = logger;
        }
        public async Task<HistoricCustomerMetric> CreateMetricOnUser(Customer customer,
                                                                   string metricCommonId,
                                                                   object value,
                                                                   bool? forceIncrementVersion)
        {
            Metric metric;
            logger.LogInformation($"Creating metric on customer {customer.Id}");
            if (await metricStore.ExistsFromCommonId(metricCommonId))
            {
                metric = await metricStore.ReadFromCommonId(metricCommonId);
            }
            else
            {
                throw new BadRequestException($"Metric {metricCommonId} does not exist");
            }

            var currentVersion = await customerMetricStore.CurrentMaximumCustomerMetricVersion(customer, metric);
            var newMetricValue = GenerateMetricValues(customer, metric, value, currentVersion + 1);
            if (forceIncrementVersion == true || currentVersion == 0) // first time or incrementing
            {
                return await HandleCreateNewMetricValue(newMetricValue);
            }
            else // check whether the value has changed before updating.
            {
                var currentMetricValue = await customerMetricStore.ReadCustomerMetric(customer, metric, currentVersion);
                if (!newMetricValue.ValuesEqual(currentMetricValue))
                {
                    return await HandleCreateNewMetricValue(newMetricValue);
                }
                else // values are equal, do don't create a new metric.
                {
                    logger.LogInformation("Skipping update to metric. Values are equal");
                    return currentMetricValue;
                }
            }
        }

        private async Task<HistoricCustomerMetric> HandleCreateNewMetricValue(HistoricCustomerMetric newMetricValue)
        {
            newMetricValue = await customerMetricStore.Create(newMetricValue);
            await SendToMetricDestinations(newMetricValue);
            await triggersWorkflows.HandleMetricValue(newMetricValue);
            await customerMetricStore.Context.SaveChanges();
            return newMetricValue;
        }

        private async Task SendToMetricDestinations(HistoricCustomerMetric newMetricValue)
        {
            var metric = newMetricValue.Metric;
            await metricStore.LoadMany(metric, _ => _.Destinations);
            var destinations = metric.Destinations;
            if (destinations != null && destinations.Any())
            {
                foreach (var dest in destinations)
                {
                    if (dest is WebhookMetricDestination webhookDestination)
                    {
                        try
                        {

                            await webhookSenderClient.Send(webhookDestination, newMetricValue);
                        }
                        catch (System.Exception ex)
                        {
                            telemetry.TrackException(ex);
                            logger.LogError($"Error sending webhook to Endpoint: {ex.Message}");
                        }
                    }
                    else if (dest is HubspotContactPropertyMetricDestination hubspotDest)
                    {
                        try
                        {
                            await hubspotPushWorkflows.SetMetricValueOnContact(hubspotDest, newMetricValue);
                        }
                        catch (System.Exception ex)
                        {
                            telemetry.TrackException(ex);
                            logger.LogError($"Error sending webhook to Hubspot: {ex.Message}");
                        }
                    }
                    else
                    {
                        throw new BadRequestException($"Cannot send a metric value to destination of type {dest?.GetType()}");
                    }
                }
            }
        }

        private HistoricCustomerMetric GenerateMetricValues(Customer user, Metric metric, object value, int version)
        {
            if (value == null)
            {
                throw new System.NullReferenceException("Metric value cannot be null");
            }

            if (metric.ValueType == Metrics.MetricValueType.Numeric || metric.ValueType == null) // null is backwards compat
            {
                if (double.TryParse(value?.ToString(), out var doubleValue))
                {
                    value = doubleValue;
                };

                if (value == null)
                {
                    throw new System.NullReferenceException("Metric value cannot be null");
                }
                else if (value is double f)
                {
                    return HistoricCustomerMetric.FromDouble(user, metric, f, version);
                }
                else if (value is int n)
                {
                    return HistoricCustomerMetric.FromDouble(user, metric, n, version);
                }
                else if (value is System.Text.Json.JsonElement jsonElement)
                {
                    if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                    {
                        if (jsonElement.TryGetInt32(out var i))
                        {
                            return HistoricCustomerMetric.FromDouble(user, metric, i, version);
                        }
                        else if (jsonElement.TryGetDouble(out var d))
                        {
                            return HistoricCustomerMetric.FromDouble(user, metric, d, version);
                        }
                    }
                }
            }


            if (metric.ValueType == Metrics.MetricValueType.Categorical || metric.ValueType == null) // null is backwards compat
            {

                if (value is string s)
                {
                    return HistoricCustomerMetric.FromString(user, metric, s, version);
                }
                else if (value is System.Text.Json.JsonElement jsonElement)
                {
                    if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String)
                    {
                        return HistoricCustomerMetric.FromString(user, metric, jsonElement.GetString(), version);
                    }
                }
            }


            throw new BadRequestException($"{value} ({value.GetType()}) cannot be parsed as metric of value type {metric.ValueType}");
        }
    }
}