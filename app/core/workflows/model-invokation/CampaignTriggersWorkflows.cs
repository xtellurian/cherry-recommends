using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core.Workflows
{
    public class CampaignTriggersWorkflows : IRecommenderMetricTriggersWorkflow, IWorkflow
    {
        private readonly ILogger<CampaignTriggersWorkflows> logger;
        private readonly ITelemetry telemetry;
        private readonly IPromotionsCampaignStore itemsRecommenderStore;
        private readonly IParameterSetCampaignStore parameterSetRecommenderStore;
        private readonly PromotionsCampaignInvokationWorkflows itemsRecommenderInvokationWorkflows;
        private readonly ParameterSetCampaignInvokationWorkflows parameterSetRecommenderInvokationWorkflows;
        private readonly IHistoricCustomerMetricStore historicMetricStore;

        public CampaignTriggersWorkflows(ILogger<CampaignTriggersWorkflows> logger,
                                            ITelemetry telemetry,
                                            IPromotionsCampaignStore itemsRecommenderStore,
                                            IParameterSetCampaignStore parameterSetRecommenderStore,
                                            PromotionsCampaignInvokationWorkflows itemsRecommenderInvokationWorkflows,
                                            ParameterSetCampaignInvokationWorkflows parameterSetRecommenderInvokationWorkflows,
                                            IHistoricCustomerMetricStore historicMetricStore)
        {
            this.logger = logger;
            this.telemetry = telemetry;
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

            await HandleNewMetricValuePromotionsCampaigns(metricValue);
            await HandleNewMetricValueParameterSetCampaigns(metricValue);
        }

        private async Task<IEnumerable<ItemsRecommendation>> HandleNewMetricValuePromotionsCampaigns(HistoricCustomerMetric metricValue)
        {
            var recommendations = new List<ItemsRecommendation>();
            await foreach (var recommender in itemsRecommenderStore.Iterate(_ => _.TriggerCollection != null && _.TargetType == PromotionCampaignTargetTypes.Customer))
            {
                var isDisabled = await itemsRecommenderInvokationWorkflows.IsCampaignDisabled(recommender);
                if (isDisabled)
                {
                    logger.LogInformation($"Skipping trigger for recommender {recommender.Id}. Campaign is disabled");
                    continue;
                }
                try
                {
                    await InvokePromotionsCampaignCustomerMetricsChangedTrigger(recommender, metricValue, recommendations);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Exception invoking items recommender Id={recommender.Id}. {ex.Message}");
                    telemetry.TrackException(ex);
                }
            }

            return recommendations;
        }

        private async Task InvokePromotionsCampaignCustomerMetricsChangedTrigger(PromotionsCampaign recommender, HistoricCustomerMetric metricValue, List<ItemsRecommendation> recommendations)
        {
            if (recommender?.TriggerCollection?.FeaturesChanged != null) // check the features changed trigger exists
            {
                if (recommender.TriggerCollection.FeaturesChanged.FeatureCommonIds?.Contains(metricValue.Metric.CommonId) == true)
                {
                    var triggerName = recommender.TriggerCollection.FeaturesChanged.Name;
                    logger.LogInformation($"Triggering an invokation for items recommender Id={recommender.Id}");
                    var recommendation = await itemsRecommenderInvokationWorkflows.InvokePromotionsCampaign(
                        recommender, new ItemsModelInputDto(metricValue.TrackedUser.CommonId), triggerName);
                    recommendations.Add(recommendation);
                }
            }
        }

        private async Task<IEnumerable<ParameterSetRecommendation>> HandleNewMetricValueParameterSetCampaigns(HistoricCustomerMetric metricValue)
        {
            var recommendations = new List<ParameterSetRecommendation>();
            var recommenders = await parameterSetRecommenderStore.Query(new EntityStoreQueryOptions<ParameterSetCampaign>(1, _ => _.TriggerCollection != null));
            logger.LogInformation($"Found {recommenders.Pagination.TotalItemCount} parameter set recommenders with Triggers");
            if (recommenders.Pagination.PageNumber > 1)
            {
                logger.LogCritical($"The Recommender Trigger workflow needs pagination support!");
            }

            foreach (var recommender in recommenders.Items)
            {
                var isDisabled = await parameterSetRecommenderInvokationWorkflows.IsCampaignDisabled(recommender);
                if (isDisabled)
                {
                    logger.LogInformation($"Skipping trigger for recommender {recommender.Id}. Campaign is disabled");
                    continue;
                }
                try
                {
                    await InvokeParameterSetCampaignMetricsChangedTrigger(recommender, metricValue, recommendations);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Exception invoking parameter set recommender Id={recommender.Id}. {ex.Message}");
                }
            }
            return recommendations;
        }

        private async Task InvokeParameterSetCampaignMetricsChangedTrigger(ParameterSetCampaign recommender, HistoricCustomerMetric metricValue, List<ParameterSetRecommendation> recommendations)
        {
            if (recommender?.TriggerCollection?.FeaturesChanged != null) // check the metrics aka features changed trigger exists
            {
                if (recommender.TriggerCollection.FeaturesChanged.FeatureCommonIds?.Contains(metricValue.Metric.CommonId) == true)
                {
                    var triggerName = recommender.TriggerCollection.FeaturesChanged.Name;
                    logger.LogInformation($"Triggering an invokation for parameterset recommender Id={recommender.Id}");
                    var recommendation = await parameterSetRecommenderInvokationWorkflows.InvokeParameterSetCampaign(
                        recommender, new ParameterSetRecommenderModelInputV1(metricValue.TrackedUser.CommonId), triggerName);
                    recommendations.Add(recommendation);
                }
            }
        }
    }
}