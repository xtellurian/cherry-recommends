using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Features.Generators;

namespace SignalBox.Core.Workflows
{
    public partial class FeatureGeneratorWorkflows : FeatureWorkflowBase, IWorkflow
    {
        private readonly ITrackedUserStore trackedUserStore;
        private readonly ITrackedUserEventStore trackedUserEventStore;
        private readonly IFeatureGeneratorStore featureGeneratorStore;
        private readonly IDateTimeProvider dateTimeProvider;

        public FeatureGeneratorWorkflows(
            ITrackedUserStore trackedUserStore,
            IFeatureStore featureStore,
            ITrackedUserEventStore trackedUserEventStore,
            IFeatureGeneratorStore featureGeneratorStore,
            IHistoricTrackedUserFeatureStore historicTrackedUserFeatureStore,
            RecommenderTriggersWorkflows triggersWorkflows,
            HubspotPushWorkflows hubspotPushWorkflows,
            IWebhookSenderClient webhookSenderClient,
            IDateTimeProvider dateTimeProvider,
            ITelemetry telemetry,
            ILogger<FeatureGeneratorWorkflows> logger)
            : base(featureStore, historicTrackedUserFeatureStore, triggersWorkflows, hubspotPushWorkflows, webhookSenderClient, telemetry, logger)
        {
            this.trackedUserStore = trackedUserStore;
            this.trackedUserEventStore = trackedUserEventStore;
            this.featureGeneratorStore = featureGeneratorStore;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<Paginated<FeatureGenerator>> GetGenerators(int page, Feature feature)
        {
            return await featureGeneratorStore.Query(page, _ => _.FeatureId == feature.Id);
        }

        public async Task<IEnumerable<FeatureGeneratorRunSummary>> RunAllFeatureGenerators()
        {
            var generators = await featureGeneratorStore.Query(1);
            List<FeatureGeneratorRunSummary> runSummaries = new List<FeatureGeneratorRunSummary>();
            foreach (var g in generators.Items)
            {
                runSummaries.Add(await RunFeatureGeneration(g));
            }

            return runSummaries;
        }

        public async Task<FeatureGeneratorRunSummary> RunFeatureGeneration(FeatureGenerator generator)
        {
            await featureGeneratorStore.Load(generator, _ => _.Feature);
            FeatureGeneratorRunSummary summary;
            switch (generator.GeneratorType)
            {
                case FeatureGeneratorTypes.MonthsSinceEarliestEvent:
                    summary = await RunMonthsSinceEarliestEventGenerator(generator);
                    break;
                case FeatureGeneratorTypes.FilterSelectAggregate:
                    summary = await RunFilterSelectAggregateGenerator(generator);
                    break;
                default:
                    throw new BadRequestException($"{generator.GeneratorType} is an unhandlable generator type");
            }

            generator.LastCompleted = dateTimeProvider.Now;
            await featureGeneratorStore.Update(generator);
            await featureGeneratorStore.Context.SaveChanges();

            return summary;
        }



    }
}