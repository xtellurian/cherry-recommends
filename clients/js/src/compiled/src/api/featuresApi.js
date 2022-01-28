import { executeFetch } from "./client/apiClient";
console.log("Deprecation Notice: Feature Generators are replaced by Metric Generators.");
export const fetchFeaturesAsync = async ({ token, page, searchTerm }) => {
    return await executeFetch({
        path: "api/Features",
        token,
        page,
        query: {
            "q.term": searchTerm,
        },
    });
};
export const fetchFeatureAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/features/${id}`,
        token,
    });
};
export const fetchFeatureTrackedUsersAsync = async ({ token, page, id }) => {
    return await executeFetch({
        path: `api/Features/${id}/TrackedUsers`,
        token,
        page,
    });
};
export const fetchFeatureTrackedUserFeaturesAsync = async ({ token, page, id, }) => {
    return await executeFetch({
        path: `api/Features/${id}/TrackedUserFeatures`,
        token,
        page,
    });
};
export const createFeatureAsync = async ({ token, feature }) => {
    return await executeFetch({
        path: "api/Features",
        token,
        method: "post",
        body: feature,
    });
};
export const deleteFeatureAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/features/${id}`,
        token,
        method: "delete",
    });
};
export const fetchTrackedUserFeaturesAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/TrackedUsers/${id}/features`,
        token,
    });
};
export const fetchTrackedUserFeatureValuesAsync = async ({ token, id, feature, version, }) => {
    return await executeFetch({
        path: `api/TrackedUsers/${id}/features/${feature}`,
        token,
        query: {
            version,
        },
    });
};
export const fetchDestinationsAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/features/${id}/Destinations`,
        token,
    });
};
export const createDestinationAsync = async ({ token, id, destination }) => {
    return await executeFetch({
        path: `api/features/${id}/Destinations`,
        token,
        method: "post",
        body: destination,
    });
};
export const deleteDestinationAsync = async ({ token, id, destinationId }) => {
    return await executeFetch({
        path: `api/features/${id}/Destinations/${destinationId}`,
        token,
        method: "delete",
    });
};
export const fetchGeneratorsAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/features/${id}/Generators`,
        token,
    });
};
