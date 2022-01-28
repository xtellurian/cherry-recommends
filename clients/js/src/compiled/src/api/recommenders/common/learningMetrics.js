import { executeFetch } from "../../client/apiClientTs";
export const fetchLearningMetricsAsync = async ({ recommenderApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
    });
};
export const setLearningMetricsAsync = async ({ recommenderApiName, token, id, useInternalId, metricIds, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
        token,
        method: "post",
        query: { useInternalId },
        body: { metricIds, useInternalId },
    });
};
