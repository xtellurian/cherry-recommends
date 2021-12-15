import { executeFetch } from "./client/apiClient";
export const fetchTouchpointsAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/Touchpoints",
        token,
        page,
    });
};
export const fetchTouchpointAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/touchpoints/${id}`,
        token,
    });
};
export const createTouchpointMetadataAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/Touchpoints",
        token,
        method: "post",
        body: payload,
    });
};
export const fetchTrackedUserTouchpointsAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/trackedusers/${id}/touchpoints`,
        token,
    });
};
export const fetchTrackedUsersInTouchpointAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/touchpoints/${id}/trackedusers`,
        token,
    });
};
export const createTrackedUserTouchpointAsync = async ({ token, id, touchpointCommonId, payload, }) => {
    return await executeFetch({
        path: `api/trackedusers/${id}/touchpoints/${touchpointCommonId}`,
        token,
        method: "post",
        body: payload,
    });
};
export const fetchTrackedUserTouchpointValuesAsync = async ({ token, id, touchpointCommonId, }) => {
    return await executeFetch({
        path: `api/trackedusers/${id}/touchpoints/${touchpointCommonId}`,
        token,
    });
};
