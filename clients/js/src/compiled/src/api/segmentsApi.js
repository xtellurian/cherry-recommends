import { executeFetch } from "./client/apiClient";
export const fetchSegmentsAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/Segments",
        token,
        page,
    });
};
export const fetchSegmentAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/segments/${id}`,
        token,
    });
};
export const createSegmentAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/Segments",
        token,
        method: "post",
        body: payload,
    });
};
