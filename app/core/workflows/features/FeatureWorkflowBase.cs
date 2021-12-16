using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Features.Destinations;

namespace SignalBox.Core.Workflows
{
    public abstract class FeatureWorkflowBase
    {
        protected readonly IFeatureStore featureStore;
        protected readonly IHistoricTrackedUserFeatureStore trackedUserFeatureStore;
        private readonly RecommenderTriggersWorkflows triggersWorkflows;
        private readonly HubspotPushWorkflows hubspotPushWorkflows;
        private readonly IWebhookSenderClient webhookSenderClient;
        private readonly ITelemetry telemetry;
        protected readonly ILogger<FeatureWorkflowBase> logger;

        protected FeatureWorkflowBase(IFeatureStore featureStore,
                                   IHistoricTrackedUserFeatureStore trackedUserFeatureStore,
                                   RecommenderTriggersWorkflows triggersWorkflows,
                                   HubspotPushWorkflows hubspotPushWorkflows,
                                   IWebhookSenderClient webhookSenderClient,
                                   ITelemetry telemetry,
                                   ILogger<FeatureWorkflowBase> logger)
        {
            this.featureStore = featureStore;
            this.trackedUserFeatureStore = trackedUserFeatureStore;
            this.triggersWorkflows = triggersWorkflows;
            this.hubspotPushWorkflows = hubspotPushWorkflows;
            this.webhookSenderClient = webhookSenderClient;
            this.telemetry = telemetry;
            this.logger = logger;
        }
        public async Task<HistoricTrackedUserFeature> CreateFeatureOnUser(Customer customer,
                                                                   string featureCommonId,
                                                                   object value,
                                                                   bool? forceIncrementVersion)
        {
            Feature feature;
            logger.LogInformation($"Creating feature on tracked user {customer.Id}");
            if (await featureStore.ExistsFromCommonId(featureCommonId))
            {
                feature = await featureStore.ReadFromCommonId(featureCommonId);
            }
            else
            {
                throw new BadRequestException($"Feature {featureCommonId} does not exist");
            }

            var currentVersion = await trackedUserFeatureStore.CurrentMaximumFeatureVersion(customer, feature);
            var newFeatureValue = GenerateFeatureValues(customer, feature, value, currentVersion + 1);
            if (forceIncrementVersion == true || currentVersion == 0) // first time or incrementing
            {
                return await HandleCreateNewFeatureValue(newFeatureValue);
            }
            else // check whether the value has changed before updating.
            {
                var currentFeatureValue = await trackedUserFeatureStore.ReadFeature(customer, feature, currentVersion);
                if (!newFeatureValue.ValuesEqual(currentFeatureValue))
                {
                    return await HandleCreateNewFeatureValue(newFeatureValue);
                }
                else // values are equal, do don't create a new feature.
                {
                    logger.LogInformation("Skipping update to Feature. Values are equal");
                    return currentFeatureValue;
                }
            }
        }

        private async Task<HistoricTrackedUserFeature> HandleCreateNewFeatureValue(HistoricTrackedUserFeature newFeatureValue)
        {
            newFeatureValue = await trackedUserFeatureStore.Create(newFeatureValue);
            await SendToFeatureDestinations(newFeatureValue);
            await triggersWorkflows.HandleFeatureValue(newFeatureValue);
            await trackedUserFeatureStore.Context.SaveChanges();
            return newFeatureValue;
        }

        private async Task SendToFeatureDestinations(HistoricTrackedUserFeature newFeatureValue)
        {
            var feature = newFeatureValue.Feature;
            await featureStore.LoadMany(feature, _ => _.Destinations);
            var destinations = feature.Destinations;
            if (destinations != null && destinations.Any())
            {
                foreach (var dest in destinations)
                {
                    if (dest is WebhookFeatureDestination webhookDestination)
                    {
                        try
                        {

                            await webhookSenderClient.Send(webhookDestination, newFeatureValue);
                        }
                        catch (System.Exception ex)
                        {
                            telemetry.TrackException(ex);
                            logger.LogError($"Error sending webhook to Endpoint: {ex.Message}");
                        }
                    }
                    else if (dest is HubspotContactPropertyFeatureDestination hubspotDest)
                    {
                        try
                        {
                            await hubspotPushWorkflows.SetFeatureValueOnContact(hubspotDest, newFeatureValue);
                        }
                        catch (System.Exception ex)
                        {
                            telemetry.TrackException(ex);
                            logger.LogError($"Error sending webhook to Hubspot: {ex.Message}");
                        }
                    }
                    else
                    {
                        throw new BadRequestException($"Cannot send a feature value to destination of type {dest?.GetType()}");
                    }
                }
            }
        }

        private HistoricTrackedUserFeature GenerateFeatureValues(Customer user, Feature feature, object value, int version)
        {
            if (double.TryParse(value?.ToString(), out var doubleValue))
            {
                value = doubleValue;
            };

            if (value == null)
            {
                throw new System.NullReferenceException("Feature value cannot be null");
            }
            else if (value is double f)
            {
                return new HistoricTrackedUserFeature(user, feature, f, version);
            }
            else if (value is int n)
            {
                return new HistoricTrackedUserFeature(user, feature, n, version);
            }
            else if (value is string s)
            {
                return new HistoricTrackedUserFeature(user, feature, s, version);
            }
            else if (value is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String)
                {
                    return new HistoricTrackedUserFeature(user, feature, jsonElement.GetString(), version);
                }
                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                {
                    if (jsonElement.TryGetInt32(out var i))
                    {
                        return new HistoricTrackedUserFeature(user, feature, i, version);
                    }
                    else if (jsonElement.TryGetDouble(out var d))
                    {
                        return new HistoricTrackedUserFeature(user, feature, d, version);
                    }
                    else
                    {
                        throw new System.ArgumentException($"{value} JsonElement of ValueKind {jsonElement.ValueKind} is an unknown feature value type");
                    }
                }
            }

            throw new System.ArgumentException($"{value} of type {value.GetType()} is an unknown feature value type");
        }

    }
}