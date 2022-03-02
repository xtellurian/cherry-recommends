


#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Workflows
{
    public class ItemsRecommenderPerformanceWorkflows : RecommenderWorkflowBase<ItemsRecommender>
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IItemsRecommendationStore recommendationStore;
        private readonly ICustomerStore customerStore;
        private readonly IHistoricCustomerMetricStore historicCustomerMetricStore;
        private readonly IItemsRecommenderPerformanceReportStore performanceReportStore;
        private readonly IRecommendableItemStore recommendableItemStore;
        private readonly ILogger<ItemsRecommenderPerformanceWorkflows> logger;

        public ItemsRecommenderPerformanceWorkflows(
            IItemsRecommenderStore store,
            IDateTimeProvider dateTimeProvider,
            IItemsRecommendationStore recommendationStore,
            IMetricStore metricStore,
            ICustomerStore customerStore,
            IHistoricCustomerMetricStore historicCustomerMetricStore,
            IIntegratedSystemStore systemStore,
            RecommenderReportImageWorkflows reportImageWorkflows,
            IItemsRecommenderPerformanceReportStore performanceReportStore,
            IRecommendableItemStore recommendableItemStore,
            ILogger<ItemsRecommenderPerformanceWorkflows> logger) : base(store, systemStore, metricStore, reportImageWorkflows)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.recommendationStore = recommendationStore;
            this.customerStore = customerStore;
            this.historicCustomerMetricStore = historicCustomerMetricStore;
            this.performanceReportStore = performanceReportStore;
            this.recommendableItemStore = recommendableItemStore;
            this.logger = logger;
        }

        public IItemsRecommenderPerformanceReportStore PerformanceReportStore => PerformanceReportStore;

        public async Task<ItemsRecommenderPerformanceReport> GetOrCalculateLatestPerfomance(string recommenderId, bool? useInternalId = null)
        {
            var recommender = await store.GetEntity(recommenderId, useInternalId);
            if (recommender.TargetMetricId == null)
            {
                throw new WorkflowException("Recommender has no target metric. Cannot calculate performance.");
            }
            await store.Load(recommender, _ => _.TargetMetric);
            await store.LoadMany(recommender, _ => _.Items);

            // return the most recent report if it exists
            if (await performanceReportStore.HasReport(recommender))
            {
                var latestReport = await performanceReportStore.ReadLatestForRecommender(recommender);

                if (latestReport.Created.AddHours(6) > dateTimeProvider.Now)
                {
                    await FixRecommmendersWithNullOrEmptyItems(recommender); // temp bug fix
                    return latestReport;
                }
            }

            // var customerIds = new HashSet<long>();
            var customerItemIds = new Dictionary<long, long>();
            var itemMetricSumValues = new Dictionary<long, double>();
            var itemRecommendationCounts = new Dictionary<long, int>();
            // ascending forces the latest recommendations to win
            await foreach (var recommendation in recommendationStore.Iterate(_ => _.RecommenderId == recommender.Id, IterateOrderBy.AscendingId))
            {
                // await recommendationStore.Load(recommendation, _ => _.Customer);
                if (recommendation.TrackedUserId.HasValue && recommendation.MaxScoreItemId.HasValue)
                {
                    // should always have a value
                    customerItemIds[recommendation.TrackedUserId.Value] = recommendation.MaxScoreItemId.Value;
                    itemMetricSumValues[recommendation.MaxScoreItemId.Value] = 0; // initialise this

                    if (!itemRecommendationCounts.ContainsKey(recommendation.MaxScoreItemId.Value))
                    {
                        itemRecommendationCounts.Add(recommendation.MaxScoreItemId.Value, 0);
                    }
                    itemRecommendationCounts[recommendation.MaxScoreItemId.Value] += 1;
                }
            }

            foreach (var customerId in customerItemIds.Keys)
            {
                var customer = await customerStore.Read(customerId);
                if (await historicCustomerMetricStore.MetricExists(customer, recommender.TargetMetric))
                {
                    var latestValue = await historicCustomerMetricStore.ReadCustomerMetric(customer, recommender.TargetMetric);
                    var itemId = customerItemIds[customerId];
                    if (latestValue.NumericValue.HasValue)
                    {
                        itemMetricSumValues[itemId] += latestValue.NumericValue.Value;
                    }
                }
                else
                {
                    logger.LogWarning("No metric values found for {customerId}", customer.Id);
                }
            }

            var itemPerformances = new List<PerformanceByItem>();
            var itemCustomerCounts = customerItemIds.Values.ValueCounts();
            foreach (var itemId in itemMetricSumValues.Keys)
            {
                itemPerformances.Add(new PerformanceByItem
                {
                    ItemId = itemId,
                    RecommendationCount = itemRecommendationCounts.GetValueOrDefault(itemId),
                    CustomerCount = itemCustomerCounts.GetValueOrDefault(itemId),
                    TargetMetricSum = itemMetricSumValues.GetValueOrDefault(itemId),
                });
            }

            var result = new ItemsRecommenderPerformanceReport(recommender, itemPerformances);
            logger.LogInformation("Generated new report. Saving results.");
            await performanceReportStore.Create(result);
            await performanceReportStore.Context.SaveChanges();

            await FixRecommmendersWithNullOrEmptyItems(recommender); // temporary bug fix
            return result;
        }

        /// <summary>
        /// Adds all items to a recommender with null items.
        /// This should be called right before returning to the caller.
        /// </summary>
        /// <param name="recommender"></param>
        private async Task FixRecommmendersWithNullOrEmptyItems(ItemsRecommender recommender)
        {
            if (recommender.Items == null || recommender.Items.Count == 0)
            {
                recommender.Items = new List<RecommendableItem>();
                // then load all the items into here
                await foreach (var item in recommendableItemStore.Iterate())
                {
                    recommender.Items.Add(item);
                }
            }
        }
    }
}
