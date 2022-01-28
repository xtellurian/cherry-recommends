using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Workflows
{
    public class RecommenderTriggersWorkflows : IWorkflow
    {
        private readonly ILogger<RecommenderTriggersWorkflows> logger;
        private readonly IItemsRecommenderStore itemsRecommenderStore;
        private readonly IParameterSetRecommenderStore parameterSetRecommenderStore;
        private readonly ItemsRecommenderInvokationWorkflows itemsRecommenderInvokationWorkflows;
        private readonly ParameterSetRecommenderInvokationWorkflows parameterSetRecommenderInvokationWorkflows;
        private readonly IHistoricCustomerMetricStore historicMetricStore;

        public RecommenderTriggersWorkflows(ILogger<RecommenderTriggersWorkflows> logger,
                                            IItemsRecommenderStore itemsRecommenderStore,
                                            IParameterSetRecommenderStore parameterSetRecommenderStore,
                                            ItemsRecommenderInvokationWorkflows itemsRecommenderInvokationWorkflows,
                                            ParameterSetRecommenderInvokationWorkflows parameterSetRecommenderInvokationWorkflows,
                                            IHistoricCustomerMetricStore historicMetricStore)
        {
            this.logger = logger;
            this.itemsRecommenderStore = itemsRecommenderStore;
            this.parameterSetRecommenderStore = parameterSetRecommenderStore;
            this.itemsRecommenderInvokationWorkflows = itemsRecommenderInvokationWorkflows;
            this.parameterSetRecommenderInvokationWorkflows = parameterSetRecommenderInvokationWorkflows;
            this.historicMetricStore = historicMetricStore;
        }

        // This method doesn't check whether the Metric Value is new
        public async Task HandleMetricValue(HistoricCustomerMetric metricValue)
        {
            if (metricValue.Customer == null)
            {
                await historicMetricStore.Load(metricValue, _ => _.Customer);
            }
            if (metricValue.Metric == null)
            {
                await historicMetricStore.Load(metricValue, _ => _.Metric);
            }

            await HandleNewMetricValueItemsRecommenders(metricValue);
            await HandleNewMetricValueParameterSetRecommenders(metricValue);
        }

        private async Task<IEnumerable<ItemsRecommendation>> HandleNewMetricValueItemsRecommenders(CustomerMetricBase metricValue)
        {
            var recommendations = new List<ItemsRecommendation>();
            var recommenders = await itemsRecommenderStore.Query(1, _ => _.TriggerCollection != null);
            logger.LogInformation($"Found {recommenders.Pagination.TotalItemCount} items recommenders with Triggers");
            if (recommenders.Pagination.PageNumber > 1)
            {
                logger.LogCritical($"The Recommender Trigger workflow needs pagination support!");
            }

            foreach (var recommender in recommenders.Items)
            {
                try
                {
                    await InvokeItemsRecommenderMetricsChangedTrigger(recommender, metricValue, recommendations);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Exception invoking items recommender Id={recommender.Id}. {ex.Message}");
                }
            }
            return recommendations;
        }

        private async Task InvokeItemsRecommenderMetricsChangedTrigger(ItemsRecommender recommender, CustomerMetricBase metricValue, List<ItemsRecommendation> recommendations)
        {
            if (recommender?.TriggerCollection?.FeaturesChanged != null) // check the features changed trigger exists
            {
                if (recommender.TriggerCollection.FeaturesChanged.FeatureCommonIds?.Contains(metricValue.Metric.CommonId) == true)
                {
                    var triggerName = recommender.TriggerCollection.FeaturesChanged.Name;
                    logger.LogInformation($"Triggering an invokation for items recommender Id={recommender.Id}");
                    var recommendation = await itemsRecommenderInvokationWorkflows.InvokeItemsRecommender(
                        recommender, new ItemsModelInputDto(metricValue.TrackedUser.CommonId), triggerName);
                    recommendations.Add(recommendation);
                }
            }
        }

        private async Task<IEnumerable<ParameterSetRecommendation>> HandleNewMetricValueParameterSetRecommenders(CustomerMetricBase metricValue)
        {
            var recommendations = new List<ParameterSetRecommendation>();
            var recommenders = await parameterSetRecommenderStore.Query(1, _ => _.TriggerCollection != null);
            logger.LogInformation($"Found {recommenders.Pagination.TotalItemCount} parameter set recommenders with Triggers");
            if (recommenders.Pagination.PageNumber > 1)
            {
                logger.LogCritical($"The Recommender Trigger workflow needs pagination support!");
            }

            foreach (var recommender in recommenders.Items)
            {
                try
                {
                    await InvokeParameterSetRecommenderMetricsChangedTrigger(recommender, metricValue, recommendations);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Exception invoking parameter set recommender Id={recommender.Id}. {ex.Message}");
                }
            }
            return recommendations;
        }

        private async Task InvokeParameterSetRecommenderMetricsChangedTrigger(ParameterSetRecommender recommender, CustomerMetricBase metricValue, List<ParameterSetRecommendation> recommendations)
        {
            if (recommender?.TriggerCollection?.FeaturesChanged != null) // check the metrics aka features changed trigger exists
            {
                if (recommender.TriggerCollection.FeaturesChanged.FeatureCommonIds?.Contains(metricValue.Metric.CommonId) == true)
                {
                    var triggerName = recommender.TriggerCollection.FeaturesChanged.Name;
                    logger.LogInformation($"Triggering an invokation for parameterset recommender Id={recommender.Id}");
                    var recommendation = await parameterSetRecommenderInvokationWorkflows.InvokeParameterSetRecommender(
                        recommender, new ParameterSetRecommenderModelInputV1(metricValue.TrackedUser.CommonId), triggerName);
                    recommendations.Add(recommendation);
                }
            }
        }
    }
}