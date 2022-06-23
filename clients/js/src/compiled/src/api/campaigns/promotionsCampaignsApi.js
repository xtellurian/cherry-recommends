import { executeFetch } from "../client/apiClient";
import * as link from "./common/linkRegisteredModels";
import * as ar from "./common/args";
import * as tv from "./common/targetvariables";
import * as il from "./common/invokationLogs";
import * as st from "./common/settings";
import * as ds from "./common/destinations";
import * as trig from "./common/trigger";
import * as lf from "./common/learningFeatures";
import * as lm from "./common/learningMetrics";
import * as ri from "./common/reportImages";
const campaignApiName = "PromotionsCampaigns";
export const fetchPromotionsCampaignsAsync = async ({ token, page, }) => {
    return await executeFetch({
        token,
        path: "api/campaigns/PromotionsCampaigns",
        page,
    });
};
export const fetchPromotionsCampaignAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}`,
        token,
    });
};
export const fetchPromotionsRecommendationsAsync = async ({ token, page, pageSize, id, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/Recommendations`,
        page,
        pageSize,
    });
};
export const deletePromotionsCampaignAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}`,
        token,
        method: "delete",
    });
};
export const createPromotionsCampaignAsync = async ({ token, payload, useInternalId, }) => {
    return await executeFetch({
        path: "api/campaigns/PromotionsCampaigns",
        token,
        method: "post",
        body: payload,
        query: { useInternalId },
    });
};
export const fetchPromotionsAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Promotions`,
        token,
    });
};
export const fetchAudienceAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Audience`,
        token,
    });
};
export const addAudienceSegmentAsync = async ({ token, useInternalId, id, segmentId, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Audience/Segments/`,
        token,
        query: { useInternalId },
        method: "post",
        body: { segmentId },
    });
};
export const removeAudienceSegmentAsync = async ({ token, id, segmentId, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Audience/Segments/${segmentId}`,
        token,
        method: "delete",
    });
};
export const addPromotionAsync = async ({ token, id, promotion, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Promotions`,
        token,
        method: "post",
        body: promotion,
    });
};
export const removePromotionAsync = async ({ token, id, promotionId, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Promotions/${promotionId}`,
        token,
        method: "delete",
    });
};
export const setBaselinePromotionAsync = async ({ token, id, promotionId, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/BaselinePromotion`,
        token,
        method: "post",
        body: { promotionId },
    });
};
export const getBaselinePromotionAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/BaselinePromotion`,
        token,
    });
};
export const createLinkRegisteredModelAsync = async ({ token, id, modelId, }) => {
    return await link.createLinkedRegisteredModelAsync({
        campaignApiName,
        id,
        modelId,
        token,
    });
};
export const fetchLinkedRegisteredModelAsync = async ({ token, id, }) => {
    return await link.fetchLinkedRegisteredModelAsync({
        campaignApiName,
        id,
        token,
    });
};
export const invokePromotionsCampaignAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Invoke`,
        token,
        method: "post",
        body: input,
    });
};
export const fetchInvokationLogsAsync = async ({ id, token, page, pageSize, }) => {
    return await il.fetchCampaignInvokationLogsAsync({
        campaignApiName,
        id,
        token,
        page,
        pageSize,
    });
};
export const fetchTargetVariablesAsync = async ({ id, token, name }) => {
    return await tv.fetchCampaignTargetVariableValuesAsync({
        campaignApiName,
        id,
        token,
        name,
    });
};
export const createTargetVariableAsync = async ({ id, token, targetVariableValue, }) => {
    return await tv.createCampaignTargetVariableValueAsync({
        campaignApiName,
        id,
        token,
        targetVariableValue,
    });
};
export const setSettingsAsync = async ({ id, token, settings, }) => {
    return await st.setSettingsAsync({
        campaignApiName,
        id,
        token,
        settings,
    });
};
export const fetchArgumentsAsync = async ({ id, token }) => {
    return await ar.fetchArgumentsAsync({
        campaignApiName,
        id,
        token,
    });
};
export const setArgumentsAsync = async ({ id, token, args, }) => {
    return await ar.setArgumentsAsync({
        campaignApiName,
        id,
        token,
        args,
    });
};
export const createChoosePromotionArgumentRuleAsync = async ({ id, useInternalId, token, rule, }) => {
    return await ar.createChoosePromotionArgumentRuleAsync({
        campaignApiName,
        id,
        token,
        useInternalId,
        rule,
    });
};
export const updateChoosePromotionArgumentRuleAsync = async ({ id, useInternalId, token, rule, ruleId, }) => {
    return await ar.updateChoosePromotionArgumentRuleAsync({
        campaignApiName,
        id,
        token,
        useInternalId,
        rule,
        ruleId,
    });
};
export const fetchChoosePromotionArgumentRulesAsync = async ({ id, useInternalId, token, }) => {
    return await ar.fetchChoosePromotionArgumentRulesAsync({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
export const createChooseSegmentArgumentRuleAsync = async ({ id, useInternalId, token, rule, }) => {
    return await ar.createChooseSegmentArgumentRuleAsync({
        campaignApiName,
        id,
        token,
        useInternalId,
        rule,
    });
};
export const updateChooseSegmentArgumentRuleAsync = async ({ id, useInternalId, token, rule, ruleId, }) => {
    return await ar.updateChooseSegmentArgumentRuleAsync({
        campaignApiName,
        id,
        token,
        useInternalId,
        rule,
        ruleId,
    });
};
export const fetchChooseSegmentArgumentRulesAsync = async ({ id, useInternalId, token, }) => {
    return await ar.fetchChooseSegmentArgumentRulesAsync({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
export const deleteArgumentRuleAsync = async ({ id, useInternalId, token, ruleId, }) => {
    return await ar.deleteArgumentRuleAsync({
        campaignApiName,
        id,
        token,
        useInternalId,
        ruleId,
    });
};
export const fetchDestinationsAsync = async ({ id, token }) => {
    return await ds.fetchDestinationsAsync({
        campaignApiName,
        id,
        token,
    });
};
export const createDestinationAsync = async ({ id, token, destination, }) => {
    return await ds.createDestinationAsync({
        campaignApiName,
        id,
        token,
        destination,
    });
};
export const removeDestinationAsync = async ({ id, token, destinationId, }) => {
    return await ds.removeDestinationAsync({
        campaignApiName,
        id,
        token,
        destinationId,
    });
};
export const fetchTriggerAsync = async ({ id, token }) => {
    return await trig.fetchTriggerAsync({
        campaignApiName,
        id,
        token,
    });
};
export const setTriggerAsync = async ({ id, token, trigger, }) => {
    return await trig.setTriggerAsync({
        campaignApiName,
        id,
        token,
        trigger,
    });
};
export const fetchLearningFeaturesAsync = async ({ id, token, useInternalId, }) => {
    return await lf.fetchLearningFeaturesAsync({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
export const setLearningFeaturesAsync = async ({ id, token, featureIds, useInternalId, }) => {
    return await lf.setLearningFeaturesAsync({
        campaignApiName,
        id,
        token,
        useInternalId,
        featureIds,
    });
};
export const fetchLearningMetricsAsync = async ({ id, token, useInternalId, }) => {
    return await lm.fetchLearningMetricsAsync({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
export const setLearningMetricsAsync = async ({ id, token, metricIds, useInternalId, }) => {
    return await lm.setLearningMetricsAsync({
        campaignApiName,
        id,
        token,
        useInternalId,
        metricIds,
    });
};
export const fetchStatisticsAsync = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Statistics`,
        token,
    });
};
export const fetchReportImageBlobUrlAsync = async ({ id, token, useInternalId, }) => {
    return await ri.fetchReportImageBlobUrlAsync({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
export const fetchPerformanceAsync = async ({ token, id, reportId, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/Performance/${reportId !== null && reportId !== void 0 ? reportId : "latest"}`,
    });
};
export const fetchPromotionOptimiserAsync = async ({ token, useInternalId, id, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/`,
    });
};
export const setAllPromotionOptimiserWeightsAsync = async ({ token, useInternalId, id, weights, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Weights/`,
        method: "post",
        body: weights,
    });
};
export const setPromotionOptimiserWeightAsync = async ({ token, useInternalId, id, weightId, weight, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Weights/${weightId}`,
        method: "post",
        body: { weight },
    });
};
export const fetchPromotionOptimiserWeightsAsync = async ({ token, useInternalId, id, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Weights/`,
    });
};
export const fetchPromotionOptimiserSegmentWeightsAsync = async ({ token, useInternalId, id, segmentId, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Segments/${segmentId}/Weights/`,
    });
};
export const setPromotionOptimiserSegmentWeightsAsync = async ({ token, useInternalId, id, segmentId, weights, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Segments/${segmentId}/Weights/`,
        method: "post",
        body: weights,
    });
};
export const setPromotionOptimiserSegmentWeightAsync = async ({ token, useInternalId, id, segmentId, weightId, weight, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Segments/${segmentId}/Weights/${weightId}`,
        method: "post",
        body: { weight },
    });
};
export const fetchPromotionOptimiserSegmentsAsync = async ({ token, useInternalId, id, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Segments/`,
        token,
        query: { useInternalId },
    });
};
export const addPromotionOptimiserSegmentAsync = async ({ token, useInternalId, id, segmentId, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Segments/`,
        token,
        query: { useInternalId },
        method: "post",
        body: { segmentId },
    });
};
export const removePromotionOptimiserSegmentAsync = async ({ token, id, segmentId, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Segments/${segmentId}`,
        token,
        method: "delete",
    });
};
export const setUseOptimiserAsync = async ({ token, useInternalId, id, useOptimiser, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/UseOptimiser`,
        method: "post",
        body: { useOptimiser },
    });
};
export const fetchCampaignChannelsAsync = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Channels`,
        token,
    });
};
export const addCampaignChannelAsync = async ({ token, id, channel, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Channels`,
        token,
        method: "post",
        body: channel,
    });
};
export const removeCampaignChannelAsync = async ({ id, token, channelId, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Channels/${channelId}`,
        token,
        method: "delete",
    });
};
export const fetchPromotionsRecommendationAsync = async ({ token, recommendationId, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/Recommendations/${recommendationId}`,
    });
};
export const fetchOffersAsync = async ({ token, page, pageSize, id, offerState, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/Offers`,
        page,
        pageSize,
        query: { offerState },
    });
};
export const fetchARPOReportAsync = async ({ token, id, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/ARPOReport`,
    });
};
export const fetchAPVReportAsync = async ({ token, id, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/APVReport`,
    });
};
export const fetchOfferConversionRateReportAsync = async ({ token, id, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/ConversionRateReport`,
    });
};
export const fetchPerformanceReportAsync = async ({ token, id, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/PerformanceReport`,
    });
};
export const fetchOfferSensitivityCurveReportAsync = async ({ token, id, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/SensitivityCurveReport`,
    });
};
