using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Metrics.Generators;

namespace SignalBox.Core.Workflows
{
    public partial class MetricGeneratorWorkflows : MetricWorkflowBase, IWorkflow
    {
        private readonly ICustomerStore customerStore;
        private readonly ICustomerEventStore trackedUserEventStore;
        private readonly IMetricGeneratorStore metricGeneratorStore;
        private readonly IDateTimeProvider dateTimeProvider;

        public MetricGeneratorWorkflows(
            ICustomerStore customerStore,
            IMetricStore metricStore,
            ICustomerEventStore trackedUserEventStore,
            IMetricGeneratorStore metricGeneratorStore,
            IHistoricCustomerMetricStore customerMetricStore,
            RecommenderTriggersWorkflows triggersWorkflows,
            HubspotPushWorkflows hubspotPushWorkflows,
            IWebhookSenderClient webhookSenderClient,
            IDateTimeProvider dateTimeProvider,
            ITelemetry telemetry,
            ILogger<MetricGeneratorWorkflows> logger)
            : base(metricStore, customerMetricStore, triggersWorkflows, hubspotPushWorkflows, webhookSenderClient, telemetry, logger)
        {
            this.customerStore = customerStore;
            this.trackedUserEventStore = trackedUserEventStore;
            this.metricGeneratorStore = metricGeneratorStore;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<Paginated<MetricGenerator>> GetGenerators(int page, Metric metric)
        {
            return await metricGeneratorStore.Query(_ => _.AggregateCustomerMetric.Metric,
                new EntityStoreQueryOptions<MetricGenerator>(page, _ => _.MetricId == metric.Id));
        }

        public async Task<IEnumerable<MetricGeneratorRunSummary>> RunAllMetricGenerators()
        {
            var generators = await metricGeneratorStore.Query(1);
            List<MetricGeneratorRunSummary> runSummaries = new List<MetricGeneratorRunSummary>();
            foreach (var g in generators.Items)
            {
                runSummaries.Add(await RunMetricGeneration(g));
            }

            return runSummaries;
        }

        public async Task<MetricGeneratorRunSummary> RunMetricGeneration(MetricGenerator generator)
        {
            await metricGeneratorStore.Load(generator, _ => _.Metric);
            MetricGeneratorRunSummary summary;
            switch (generator.GeneratorType)
            {
                case MetricGeneratorTypes.MonthsSinceEarliestEvent:
                    summary = await RunMonthsSinceEarliestEventGenerator(generator);
                    break;
                case MetricGeneratorTypes.FilterSelectAggregate:
                    summary = await RunFilterSelectAggregateGenerator(generator);
                    break;
                default:
                    throw new BadRequestException($"{generator.GeneratorType} is an unhandlable generator type");
            }

            return summary;
        }



    }
}