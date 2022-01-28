import { executeFetch } from "./client/apiClientTs";
export const fetchMetricGeneratorsAsync = async ({ page, token, }) => {
    return await executeFetch({
        path: "api/MetricGenerators",
        token,
        page,
    });
};
export const createMetricGeneratorAsync = async ({ token, generator, }) => {
    return await executeFetch({
        path: "api/MetricGenerators",
        token,
        method: "post",
        body: generator,
    });
};
export const deleteMetricGeneratorAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/MetricGenerators/${id}`,
        token,
        method: "delete",
    });
};
export const manualTriggerMetricGeneratorsAsync = async ({ token, id, }) => {
    return await executeFetch({
        token,
        path: `api/MetricGenerators/${id}/Trigger`,
        method: "post",
        body: {},
    });
};
