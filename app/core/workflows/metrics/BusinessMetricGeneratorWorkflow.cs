using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Metrics;
using SignalBox.Core.Metrics.Destinations;
using SignalBox.Core.Metrics.Generators;

namespace SignalBox.Core.Workflows
{
#nullable enable
    public partial class BusinessMetricGeneratorWorkflow : IWorkflow
    {
        private readonly IBusinessMetricValueStore businessMetricValueStore;
        private readonly IBusinessStore businessStore;
        private readonly ICustomerEventStore customerEventStore;
        private readonly IMetricStore metricStore;
        private readonly IMetricGeneratorStore metricGeneratorStore;
        private readonly IWebhookSenderClient webhookSenderClient;
        private readonly ITelemetry telemetry;
        private readonly FilterSelectAggregateWorkflow aggregateWorkflow;
        private readonly ILogger<BusinessMetricGeneratorWorkflow> logger;
        public BusinessMetricGeneratorWorkflow(IBusinessMetricValueStore businessMetricValueStore,
                                               IBusinessStore businessStore,
                                               ICustomerEventStore customerEventStore,
                                               IMetricStore metricStore,
                                               IMetricGeneratorStore metricGeneratorStore,
                                               FilterSelectAggregateWorkflow aggregateWorkflow,
                                               IWebhookSenderClient webhookSenderClient,
                                               ITelemetry telemetry,
                                               ILogger<BusinessMetricGeneratorWorkflow> logger)
        {
            this.businessMetricValueStore = businessMetricValueStore;
            this.businessStore = businessStore;
            this.customerEventStore = customerEventStore;
            this.metricStore = metricStore;
            this.metricGeneratorStore = metricGeneratorStore;
            this.aggregateWorkflow = aggregateWorkflow;
            this.webhookSenderClient = webhookSenderClient;
            this.telemetry = telemetry;
            this.logger = logger;
        }

        public async Task<MetricGeneratorRunSummary> RunBusinessMetricGeneration(MetricGenerator generator)
        {
            if (generator.GeneratorType != MetricGeneratorTypes.FilterSelectAggregate)
            {
                throw new BadRequestException($"{generator.GeneratorType} is an unhandled business generator type");
            }

            return await RunBusinessFilterSelectAggregateGenerator(generator);
        }

        protected async Task<MetricGeneratorRunSummary> RunBusinessFilterSelectAggregateGenerator(MetricGenerator generator)
        {
            logger.LogInformation($"Running FilterSelectAggregate generator: {generator.Id} for Metric {generator.MetricId}");

            await metricGeneratorStore.Load(generator, _ => _.Metric);
            var steps = new List<FilterSelectAggregateStep>(generator.FilterSelectAggregateSteps).OrderBy(_ => _.Order);

            logger.LogInformation($"There are {steps.Count()} steps in generator {generator.Id}");
            var summary = new MetricGeneratorRunSummary(0);

            await foreach (var business in businessStore.Iterate())
            {
                logger.LogInformation($"Running generator {generator.Id} for metric {generator.MetricId} and Business {business.Id}.");
                var context = new FilterSelectAggregateContext(business, generator.Metric, customerEventStore)
                {
                    Steps = steps.ToList(),
                };
                DateTimeOffset since = DateTimeOffset.Now.DateTimeSince(generator.TimeWindow ?? MetricGeneratorTimeWindow.AllTime);

                try
                {
                    var value = await GenerateMetricValueForBusiness(business, generator.Metric, context, since);
                    if (value != null)
                    {
                        summary.TotalWrites += 1;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    throw new WorkflowException($"Error running Filter Select Aggregate generator {generator.Id}", ex);
                }
            }

            return summary;
        }

        private async Task<BusinessMetricValue?> GenerateMetricValueForBusiness(Business business, Metric metric, FilterSelectAggregateContext context, DateTimeOffset? since = null)
        {
            var finalValue = await aggregateWorkflow.RunFilterSelectAggregateWorkflow(context, since);
            if (finalValue != null)
            {
                return await CreateMetricOnBusiness(business, metric, finalValue);
            }

            return null;
        }

        private async Task<BusinessMetricValue?> CreateMetricOnBusiness(Business business, Metric metric, double? value)
        {
            BusinessMetricValue? newMetric = null;
            var latest = await businessMetricValueStore.LatestMetricValue(business, metric);
            if (value.HasValue && (latest == null || latest.NumericValue != value))
            {
                logger.LogInformation("Updating metric {metricId}", metric.Id);
                var version = (latest?.Version ?? 0) + 1;

                newMetric = await businessMetricValueStore.Create(new BusinessMetricValue(business, metric, value.Value, version));
                await businessMetricValueStore.Context.SaveChanges();

                await SendToMetricDestinations(newMetric);
            }
            else
            {
                logger.LogInformation("Not updating metric {metricId} for business {businessCommonId}", metric.Id, business.CommonId);
            }

            return newMetric;
        }

        private async Task SendToMetricDestinations(BusinessMetricValue newMetricValue)
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
                    else
                    {
                        throw new BadRequestException($"Cannot send a metric value to destination of type {dest?.GetType()}");
                    }
                }
            }
        }
    }
}