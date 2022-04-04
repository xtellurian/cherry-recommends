import { current } from "./axiosInstance";
import { getBaseUrl } from "./baseUrl";
import { headers } from "./headers";
import { handleErrorResponse } from "../../utilities/errorHandling";
export const executeFetch = async ({ token, apiKey, path, page, pageSize, body, method, query, } = { method: "get", path: "/" }) => {
    const baseUrl = getBaseUrl();
    const client = current({ baseUrl: baseUrl });
    const params = new URLSearchParams();
    for (const [key, value] of Object.entries(query || {})) {
        if (key && value) {
            params.append(key, value);
        }
    }
    if (page) {
        params.append("p.page", `${page}`);
    }
    if (pageSize) {
        params.append("p.pageSize", `${pageSize}`);
    }
    if (apiKey) {
        params.append("apiKey", `${apiKey}`);
    }
    let response;
    try {
        response = await client({
            method,
            url: path,
            params,
            headers: headers(token, null),
            data: body,
        });
    }
    catch (ex) {
        // something failed. ther server responded outside of 2xx
        // if we always resolve the promise, then this is always true
        console.error(ex);
        throw ex;
    }
    // happy path
    if (response.status <= 299) {
        return response.data;
    }
    else {
        return await handleErrorResponse(response);
    }
};
