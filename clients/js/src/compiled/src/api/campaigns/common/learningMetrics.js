import { executeFetch } from "../../client/apiClient";
export const fetchLearningMetricsAsync = async ({ campaignApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/${campaignApiName}/${id}/LearningFeatures`,
    });
};
export const setLearningMetricsAsync = async ({ campaignApiName, token, id, useInternalId, metricIds, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/LearningFeatures`,
        token,
        method: "post",
        query: { useInternalId },
        body: { metricIds, useInternalId },
    });
};
