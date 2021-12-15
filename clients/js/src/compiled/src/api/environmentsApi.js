import { setDefaultEnvironmentId as setEnv } from "./client/headers";
import { executeFetch } from "./client/apiClient";
export const fetchEnvironmentsAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/Environments",
        token,
        page,
    });
};
export const createEnvironmentAsync = async ({ token, environment }) => {
    return await executeFetch({
        path: "api/Environments",
        token,
        method: "post",
        body: environment,
    });
};
export const deleteEnvironmentAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Environments/${id}`,
        token,
        method: "delete",
    });
};
export const setDefaultEnvironmentId = setEnv;
