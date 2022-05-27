import { executeFetch } from "../client/apiClient";
import * as link from "./common/linkRegisteredModels";
import * as tv from "./common/targetvariables";
import * as il from "./common/invokationLogs";
import * as ar from "./common/args";
import * as st from "./common/settings";
import * as ds from "./common/destinations";
import * as trig from "./common/trigger";
import * as lf from "./common/learningFeatures";
import * as lm from "./common/learningMetrics";
import * as ri from "./common/reportImages";
const campaignApiName = "ParameterSetCampaigns";
export const fetchParameterSetCampaignsAsync = async ({ token, page, }) => {
    return await executeFetch({
        path: "api/campaigns/ParameterSetCampaigns",
        token,
        page,
    });
};
export const fetchParameterSetCampaignAsync = async ({ token, id, searchTerm, }) => {
    return await executeFetch({
        path: `api/campaigns/ParameterSetCampaigns/${id}`,
        token,
        query: {
            "q.term": searchTerm,
        },
    });
};
export const createParameterSetCampaignAsync = async ({ token, payload, }) => {
    return await executeFetch({
        path: "api/campaigns/ParameterSetCampaigns",
        token,
        method: "post",
        body: payload,
    });
};
export const deleteParameterSetCampaignAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/ParameterSetCampaigns/${id}`,
        token,
        method: "delete",
    });
};
export const fetchParameterSetRecommendationsAsync = async ({ token, page, pageSize, id, }) => {
    return await executeFetch({
        path: `api/campaigns/ParameterSetCampaigns/${id}/recommendations`,
        token,
        page,
        pageSize,
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
export const invokeParameterSetCampaignAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/campaigns/ParameterSetCampaigns/${id}/invoke`,
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
        path: `api/campaigns/ParameterSetCampaigns/${id}/Statistics`,
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
