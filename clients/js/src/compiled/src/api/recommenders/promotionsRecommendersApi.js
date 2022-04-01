import { executeFetch } from "../client/apiClientTs";
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
const recommenderApiName = "PromotionsRecommenders";
export const fetchPromotionsRecommendersAsync = async ({ token, page, }) => {
    return await executeFetch({
        token,
        path: "api/recommenders/PromotionsRecommenders",
        page,
    });
};
export const fetchPromotionsRecommenderAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}`,
        token,
    });
};
export const fetchPromotionsRecommendationsAsync = async ({ token, page, pageSize, id, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/PromotionsRecommenders/${id}/Recommendations`,
        page,
        pageSize,
    });
};
export const deletePromotionsRecommenderAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}`,
        token,
        method: "delete",
    });
};
export const createPromotionsRecommenderAsync = async ({ token, payload, useInternalId, }) => {
    return await executeFetch({
        path: "api/recommenders/PromotionsRecommenders",
        token,
        method: "post",
        body: payload,
        query: { useInternalId },
    });
};
export const fetchPromotionsAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Promotions`,
        token,
    });
};
export const fetchAudienceAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Audience`,
        token,
    });
};
export const addPromotionAsync = async ({ token, id, promotion, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Promotions`,
        token,
        method: "post",
        body: promotion,
    });
};
export const removePromotionAsync = async ({ token, id, promotionId, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Promotions/${promotionId}`,
        token,
        method: "post",
    });
};
export const setBaselinePromotionAsync = async ({ token, id, promotionId, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/BaselinePromotion`,
        token,
        method: "post",
        body: { promotionId },
    });
};
export const getBaselinePromotionAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/BaselinePromotion`,
        token,
    });
};
export const createLinkRegisteredModelAsync = async ({ token, id, modelId, }) => {
    return await link.createLinkedRegisteredModelAsync({
        recommenderApiName,
        id,
        modelId,
        token,
    });
};
export const fetchLinkedRegisteredModelAsync = async ({ token, id, }) => {
    return await link.fetchLinkedRegisteredModelAsync({
        recommenderApiName,
        id,
        token,
    });
};
export const invokePromotionsRecommenderAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Invoke`,
        token,
        method: "post",
        body: input,
    });
};
export const fetchInvokationLogsAsync = async ({ id, token, page, }) => {
    return await il.fetchRecommenderInvokationLogsAsync({
        recommenderApiName,
        id,
        token,
        page,
    });
};
export const fetchTargetVariablesAsync = async ({ id, token, name }) => {
    return await tv.fetchRecommenderTargetVariableValuesAsync({
        recommenderApiName,
        id,
        token,
        name,
    });
};
export const createTargetVariableAsync = async ({ id, token, targetVariableValue, }) => {
    return await tv.createRecommenderTargetVariableValueAsync({
        recommenderApiName,
        id,
        token,
        targetVariableValue,
    });
};
export const setSettingsAsync = async ({ id, token, settings, }) => {
    return await st.setSettingsAsync({
        recommenderApiName,
        id,
        token,
        settings,
    });
};
export const setArgumentsAsync = async ({ id, token, args, }) => {
    return await ar.setArgumentsAsync({
        recommenderApiName,
        id,
        token,
        args,
    });
};
export const fetchDestinationsAsync = async ({ id, token }) => {
    return await ds.fetchDestinationsAsync({
        recommenderApiName,
        id,
        token,
    });
};
export const createDestinationAsync = async ({ id, token, destination, }) => {
    return await ds.createDestinationAsync({
        recommenderApiName,
        id,
        token,
        destination,
    });
};
export const removeDestinationAsync = async ({ id, token, destinationId, }) => {
    return await ds.removeDestinationAsync({
        recommenderApiName,
        id,
        token,
        destinationId,
    });
};
export const fetchTriggerAsync = async ({ id, token }) => {
    return await trig.fetchTriggerAsync({
        recommenderApiName,
        id,
        token,
    });
};
export const setTriggerAsync = async ({ id, token, trigger, }) => {
    return await trig.setTriggerAsync({
        recommenderApiName,
        id,
        token,
        trigger,
    });
};
export const fetchLearningFeaturesAsync = async ({ id, token, useInternalId, }) => {
    return await lf.fetchLearningFeaturesAsync({
        recommenderApiName,
        id,
        token,
        useInternalId,
    });
};
export const setLearningFeaturesAsync = async ({ id, token, featureIds, useInternalId, }) => {
    return await lf.setLearningFeaturesAsync({
        recommenderApiName,
        id,
        token,
        useInternalId,
        featureIds,
    });
};
export const fetchLearningMetricsAsync = async ({ id, token, useInternalId, }) => {
    return await lm.fetchLearningMetricsAsync({
        recommenderApiName,
        id,
        token,
        useInternalId,
    });
};
export const setLearningMetricsAsync = async ({ id, token, metricIds, useInternalId, }) => {
    return await lm.setLearningMetricsAsync({
        recommenderApiName,
        id,
        token,
        useInternalId,
        metricIds,
    });
};
export const fetchStatisticsAsync = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Statistics`,
        token,
    });
};
export const fetchReportImageBlobUrlAsync = async ({ id, token, useInternalId, }) => {
    return await ri.fetchReportImageBlobUrlAsync({
        recommenderApiName,
        id,
        token,
        useInternalId,
    });
};
export const fetchPerformanceAsync = async ({ token, id, reportId, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/PromotionsRecommenders/${id}/Performance/${reportId !== null && reportId !== void 0 ? reportId : "latest"}`,
    });
};
export const fetchPromotionOptimiserAsync = async ({ token, useInternalId, id, }) => {
    return await executeFetch({
        token,
        useInternalId,
        path: `api/recommenders/PromotionsRecommenders/${id}/Optimiser/`,
    });
};
export const setAllPromotionOptimiserWeightsAsync = async ({ token, useInternalId, id, weights, }) => {
    return await executeFetch({
        token,
        useInternalId,
        path: `api/recommenders/PromotionsRecommenders/${id}/Optimiser/Weights/`,
        method: "post",
        body: weights,
    });
};
export const setPromotionOptimiserWeightAsync = async ({ token, useInternalId, id, weightId, weight, }) => {
    return await executeFetch({
        token,
        useInternalId,
        path: `api/recommenders/PromotionsRecommenders/${id}/Optimiser/Weights/${weightId}`,
        method: "post",
        body: { weight },
    });
};
export const setUseOptimiserAsync = async ({ token, useInternalId, id, useOptimiser, }) => {
    return await executeFetch({
        token,
        useInternalId,
        path: `api/recommenders/PromotionsRecommenders/${id}/UseOptimiser`,
        method: "post",
        body: { useOptimiser },
    });
};
export const fetchRecommenderChannelsAsync = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Channels`,
        token,
    });
};
export const addRecommenderChannelAsync = async ({ token, id, channel, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Channels`,
        token,
        method: "post",
        body: channel,
    });
};
export const removeRecommenderChannelAsync = async ({ id, token, channelId, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Channels/${channelId}`,
        token,
        method: "delete",
    });
};
