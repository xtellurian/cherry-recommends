import { executeFetch } from "./client/apiClientTs";
export const fetchMetricsAsync = async ({ token, page, searchTerm, }) => {
    return await executeFetch({
        path: "api/Metrics",
        token,
        page,
        query: {
            "q.term": searchTerm,
        },
    });
};
export const fetchMetricAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}`,
        token,
    });
};
export const fetchMetricCustomersAsync = async ({ token, page, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Customers`,
        token,
        page,
    });
};
export const fetchMetricCustomerMetricsAsync = async ({ token, page, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/CustomerMetrics`,
        token,
        page,
    });
};
export const createMetricAsync = async ({ token, metric, }) => {
    return await executeFetch({
        path: "api/Metrics",
        token,
        method: "post",
        body: metric,
    });
};
export const deleteMetricAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Metrics/${id}`,
        token,
        method: "delete",
    });
};
export const fetchCustomersMetricsAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Customers/${id}/Metrics`,
        token,
    });
};
export const fetchCustomersMetricAsync = async ({ token, id, metricId, version, }) => {
    return await executeFetch({
        path: `api/Customers/${id}/Metrics/${metricId}`,
        token,
        query: {
            version,
        },
    });
};
export const fetchAggregateMetricValuesNumericAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/AggregateMetricValuesNumeric`,
        token,
    });
};
export const fetchAggregateMetricValuesStringAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/AggregateMetricValuesString`,
        token,
    });
};
export const fetchDestinationsAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Destinations`,
        token,
    });
};
export const fetchExportCustomers = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/ExportCustomers`,
        token,
    });
};
export const createDestinationAsync = async ({ token, id, destination, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Destinations`,
        token,
        method: "post",
        body: destination,
    });
};
export const deleteDestinationAsync = async ({ token, id, destinationId, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Destinations/${destinationId}`,
        token,
        method: "delete",
    });
};
export const fetchGeneratorsAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Generators`,
        token,
    });
};
