using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class FeatureGeneratorWorkflows : FeatureWorkflowBase, IWorkflow
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

        public async Task<long> RunAllFeatureGenerators()
        {
            var generators = await featureGeneratorStore.Query(1);
            long totalWrites = 0;
            foreach (var g in generators.Items)
            {
                totalWrites += await RunFeatureGeneration(g);
            }
            return totalWrites;
        }

        public async Task<int> RunFeatureGeneration(FeatureGenerator generator)
        {
            await featureGeneratorStore.Load(generator, _ => _.Feature);
            switch (generator.GeneratorType)
            {
                case FeatureGeneratorTypes.MonthsSinceEarliestEvent:
                    return await RunMonthsSinceEarliestEventGenerator(generator);
                default:
                    throw new BadRequestException($"{generator.GeneratorType} is an unhandlable generator type");
            }
        }

        private int CalcDeltaMonths(DateTimeOffset startDate, DateTimeOffset endDate) => ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month;
        private async Task<int> RunMonthsSinceEarliestEventGenerator(FeatureGenerator generator)
        {
            var page = 1;
            var hasNextPage = true;
            var now = dateTimeProvider.Now;
            var totalWrites = 0;
            while (hasNextPage)
            {
                var query = await trackedUserStore.Query(page++);
                hasNextPage = query.Pagination.HasNextPage;
                logger.LogInformation($"Page {query.Pagination.PageNumber} of {query.Pagination.PageCount} tracked user pages");
                foreach (var tu in query.Items)
                {
                    try
                    {
                        var minTimestamp = await trackedUserEventStore.Min(_ => _.TrackedUserId == tu.Id, _ => _.Timestamp);
                        var delta = CalcDeltaMonths(minTimestamp, now);
                        await base.CreateFeatureOnUser(tu, generator.Feature.CommonId, delta, false);
                        totalWrites++;
                    }
                    catch (InvalidStorageAccessException)
                    {
                        logger.LogInformation($"Skipping tracked user {tu.Id} - has no min timestamp");
                    }
                    catch (System.Exception ex)
                    {
                        logger.LogError($"Something went wrong during feature gen. Page Number = {query.Pagination.PageNumber}", ex);
                        logger.LogError(ex.GetType().ToString());
                        logger.LogError(ex.Message);
                        throw new WorkflowException("Error creating feature", ex);
                    }
                }
            }

            logger.LogInformation($"Finished feauture generator workflow with {totalWrites} total writes");

            return totalWrites;
        }
    }
}