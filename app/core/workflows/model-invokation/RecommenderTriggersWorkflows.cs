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
        private readonly IHistoricTrackedUserFeatureStore historicFeatureStore;

        public RecommenderTriggersWorkflows(ILogger<RecommenderTriggersWorkflows> logger,
                                            IItemsRecommenderStore itemsRecommenderStore,
                                            IParameterSetRecommenderStore parameterSetRecommenderStore,
                                            ItemsRecommenderInvokationWorkflows itemsRecommenderInvokationWorkflows,
                                            ParameterSetRecommenderInvokationWorkflows parameterSetRecommenderInvokationWorkflows,
                                            IHistoricTrackedUserFeatureStore historicFeatureStore)
        {
            this.logger = logger;
            this.itemsRecommenderStore = itemsRecommenderStore;
            this.parameterSetRecommenderStore = parameterSetRecommenderStore;
            this.itemsRecommenderInvokationWorkflows = itemsRecommenderInvokationWorkflows;
            this.parameterSetRecommenderInvokationWorkflows = parameterSetRecommenderInvokationWorkflows;
            this.historicFeatureStore = historicFeatureStore;
        }

        // This method doesn't check whether the Feature Value is new
        public async Task HandleFeatureValue(HistoricTrackedUserFeature featureValue)
        {
            if (featureValue.TrackedUser == null)
            {
                await historicFeatureStore.Load(featureValue, _ => _.TrackedUser);
            }
            if (featureValue.Feature == null)
            {
                await historicFeatureStore.Load(featureValue, _ => _.Feature);
            }

            await HandleNewFeatureValueItemsRecommenders(featureValue);
            await HandleNewFeatureValueParameterSetRecommenders(featureValue);
        }

        private async Task<IEnumerable<ItemsRecommendation>> HandleNewFeatureValueItemsRecommenders(TrackedUserFeatureBase featureValue)
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
                    await InvokeItemsRecommenderFeaturesChangedTrigger(recommender, featureValue, recommendations);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Exception invoking items recommender Id={recommender.Id}. {ex.Message}");
                }
            }
            return recommendations;
        }

        private async Task InvokeItemsRecommenderFeaturesChangedTrigger(ItemsRecommender recommender, TrackedUserFeatureBase featureValue, List<ItemsRecommendation> recommendations)
        {
            if (recommender?.TriggerCollection?.FeaturesChanged != null) // check the features changed trigger exists
            {
                if (recommender.TriggerCollection.FeaturesChanged.FeatureCommonIds?.Contains(featureValue.Feature.CommonId) == true)
                {
                    var triggerName = recommender.TriggerCollection.FeaturesChanged.Name;
                    logger.LogInformation($"Triggering an invokation for items recommender Id={recommender.Id}");
                    var recommendation = await itemsRecommenderInvokationWorkflows.InvokeItemsRecommender(
                        recommender, new ItemsModelInputDto(featureValue.TrackedUser.CommonId), triggerName);
                    recommendations.Add(recommendation);
                }
            }
        }

        private async Task<IEnumerable<ParameterSetRecommendation>> HandleNewFeatureValueParameterSetRecommenders(TrackedUserFeatureBase featureValue)
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
                    await InvokeParameterSetRecommenderFeaturesChangedTrigger(recommender, featureValue, recommendations);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Exception invoking parameter set recommender Id={recommender.Id}. {ex.Message}");
                }
            }
            return recommendations;
        }

        private async Task InvokeParameterSetRecommenderFeaturesChangedTrigger(ParameterSetRecommender recommender, TrackedUserFeatureBase featureValue, List<ParameterSetRecommendation> recommendations)
        {
            if (recommender?.TriggerCollection?.FeaturesChanged != null) // check the features changed trigger exists
            {
                if (recommender.TriggerCollection.FeaturesChanged.FeatureCommonIds?.Contains(featureValue.Feature.CommonId) == true)
                {
                    var triggerName = recommender.TriggerCollection.FeaturesChanged.Name;
                    logger.LogInformation($"Triggering an invokation for parameterset recommender Id={recommender.Id}");
                    var recommendation = await parameterSetRecommenderInvokationWorkflows.InvokeParameterSetRecommender(
                        recommender, new ParameterSetRecommenderModelInputV1(featureValue.TrackedUser.CommonId), triggerName);
                    recommendations.Add(recommendation);
                }
            }
        }
    }
}