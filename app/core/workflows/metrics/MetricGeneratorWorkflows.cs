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
        private readonly FilterSelectAggregateWorkflow aggregateWorkflow;

        public MetricGeneratorWorkflows(
            ICustomerStore customerStore,
            IMetricStore metricStore,
            ICustomerEventStore trackedUserEventStore,
            IMetricGeneratorStore metricGeneratorStore,
            IHistoricCustomerMetricStore customerMetricStore,
            IRecommenderMetricTriggersWorkflow triggersWorkflows,
            FilterSelectAggregateWorkflow aggregateWorkflow,
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
            this.aggregateWorkflow = aggregateWorkflow;
        }

        public async Task<Paginated<MetricGenerator>> GetGenerators(IPaginate paginated, Metric metric)
        {
            // need to load related entities
            var generators = await metricGeneratorStore.Query(new EntityStoreQueryOptions<MetricGenerator>(paginated, _ => _.MetricId == metric.Id));
            foreach (var g in generators.Items)
            {
                if (g.AggregateCustomerMetric?.MetricId != null)
                {
                    await metricStore.Read(g.AggregateCustomerMetric.MetricId); // load from DB
                }
                if (g.JoinTwoMetrics?.Metric1Id != null)
                {
                    await metricStore.Read(g.JoinTwoMetrics.Metric1Id); // load from db
                }
                if (g.JoinTwoMetrics?.Metric2Id != null)
                {
                    await metricStore.Read(g.JoinTwoMetrics.Metric2Id); // load from db
                }
            }
            return generators;
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