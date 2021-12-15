import { executeFetch } from "../client/apiClient";
export const getPropertiesAsync = async ({ api, token, id }) => {
    return await executeFetch({
        path: `api/${api}/${id}/Properties`,
        token,
    });
};
export const setPropertiesAsync = async ({ api, token, id, properties }) => {
    return await executeFetch({
        path: `api/${api}/${id}/Properties`,
        token,
        method: "post",
        body: properties,
    });
};
