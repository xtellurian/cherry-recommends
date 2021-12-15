import { executeFetch } from "../client/apiClient";
import * as link from "./common/linkRegisteredModels";
import * as actions from "./common/trackedUserActions";
import * as eh from "./common/errorHandling";
import * as tv from "./common/targetvariables";
import * as il from "./common/invokationLogs";
import * as ar from "./common/args";
import * as st from "./common/settings";
import * as ds from "./common/destinations";
import * as trig from "./common/trigger";
import * as lf from "./common/learningFeatures";
const recommenderApiName = "ParameterSetRecommenders";
export const fetchParameterSetRecommendersAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/recommenders/ParameterSetRecommenders",
        token,
        page,
    });
};
export const fetchParameterSetRecommenderAsync = async ({ token, id, searchTerm, }) => {
    return await executeFetch({
        path: `api/recommenders/ParameterSetRecommenders/${id}`,
        token,
        query: {
            "q.term": searchTerm,
        },
    });
};
export const createParameterSetRecommenderAsync = async ({ token, payload, }) => {
    return await executeFetch({
        path: "api/recommenders/ParameterSetRecommenders",
        token,
        method: "post",
        body: payload,
    });
};
export const deleteParameterSetRecommenderAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/recommenders/ParameterSetRecommenders/${id}`,
        token,
        method: "delete",
    });
};
export const fetchParameterSetRecommendationsAsync = async ({ token, page, id, }) => {
    return await executeFetch({
        path: `api/recommenders/ParameterSetRecommenders/${id}/recommendations`,
        token,
        page,
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
export const fetchLinkedRegisteredModelAsync = async ({ token, id }) => {
    return await link.fetchLinkedRegisteredModelAsync({
        recommenderApiName,
        id,
        token,
    });
};
export const invokeParameterSetRecommenderAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/recommenders/ParameterSetRecommenders/${id}/invoke`,
        token,
        method: "post",
        body: input,
    });
};
export const fetchInvokationLogsAsync = async ({ id, token, page }) => {
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
export const updateErrorHandlingAsync = async ({ id, token, errorHandling, }) => {
    return await eh.updateErrorHandlingAsync({
        recommenderApiName,
        id,
        token,
        errorHandling,
    });
};
export const setSettingsAsync = async ({ id, token, settings }) => {
    return await st.setSettingsAsync({
        recommenderApiName,
        id,
        token,
        settings,
    });
};
export const fetchRecommenderTrackedUserActionsAsync = async ({ id, token, page, revenueOnly, }) => {
    return await actions.fetchRecommenderTrackedUserActionsAsync({
        recommenderApiName,
        id,
        token,
        page,
        revenueOnly,
    });
};
export const setArgumentsAsync = async ({ id, token, args }) => {
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
export const createDestinationAsync = async ({ id, token, destination }) => {
    return await ds.createDestinationAsync({
        recommenderApiName,
        id,
        token,
        destination,
    });
};
export const removeDestinationAsync = async ({ id, token, destinationId }) => {
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
export const setTriggerAsync = async ({ id, token, trigger }) => {
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
