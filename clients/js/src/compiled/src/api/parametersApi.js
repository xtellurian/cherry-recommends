import { executeFetch } from "./client/apiClient";
export const fetchParametersAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/Parameters",
        token,
        page,
    });
};
export const fetchParameterAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/parameters/${id}`,
        token,
    });
};
export const deleteParameterAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/parameters/${id}`,
        token,
        method: "delete",
    });
};
export const createParameterAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/Parameters",
        token,
        method: "post",
        body: payload,
    });
};
