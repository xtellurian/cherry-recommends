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
const recommenderApiName = "ItemsRecommenders";
export const fetchItemsRecommendersAsync = async ({ token, page, }) => {
    return await executeFetch({
        token,
        path: "api/recommenders/ItemsRecommenders",
        page,
    });
};
export const fetchItemsRecommenderAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}`,
        token,
    });
};
export const fetchItemsRecommendationsAsync = async ({ token, page, pageSize, id, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/ItemsRecommenders/${id}/Recommendations`,
        page,
        pageSize,
    });
};
export const deleteItemsRecommenderAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}`,
        token,
        method: "delete",
    });
};
export const createItemsRecommenderAsync = async ({ token, payload, useInternalId, }) => {
    return await executeFetch({
        path: "api/recommenders/ItemsRecommenders",
        token,
        method: "post",
        body: payload,
        query: { useInternalId },
    });
};
export const fetchItemsAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/Items`,
        token,
    });
};
export const addItemAsync = async ({ token, id, item }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/Items`,
        token,
        method: "post",
        body: item,
    });
};
export const removeItemAsync = async ({ token, id, itemId, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/Items/${itemId}`,
        token,
        method: "post",
    });
};
export const setBaselineItemAsync = async ({ token, id, itemId, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/BaselineItem`,
        token,
        method: "post",
        body: { itemId },
    });
};
export const setDefaultItemAsync = setBaselineItemAsync; // backwards compat
export const getBaselineItemAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/BaselineItem`,
        token,
    });
};
export const getDefaultItemAsync = getBaselineItemAsync; // backwards compat
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
export const invokeItemsRecommenderAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/Invoke`,
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
        path: `api/recommenders/ItemsRecommenders/${id}/Statistics`,
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
        path: `api/recommenders/ItemsRecommenders/${id}/Performance/${reportId !== null && reportId !== void 0 ? reportId : "latest"}`,
    });
};
