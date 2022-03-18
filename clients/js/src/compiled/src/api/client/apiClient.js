import { getUrl } from "./baseUrl";
import { headers } from "./headers";
import { handleErrorResponse, handleErrorFetch, } from "../../utilities/errorHandling";
import logger from "../logging/logger";
export const executeFetch = async ({ token, apiKey, path, page, pageSize, body, method, query, }) => {
    const url = getUrl(path);
    const q = new URLSearchParams();
    for (const [key, value] of Object.entries(query || {})) {
        if (key && value) {
            q.append(key, value);
        }
    }
    if (page) {
        q.append("p.page", `${page}`);
    }
    if (pageSize) {
        q.append("p.pageSize", `${pageSize}`);
    }
    if (apiKey) {
        q.append("apiKey", `${apiKey}`);
    }
    const qs = q.toString();
    const fullUrl = `${url}?${qs}`;
    logger.debug(`Fetch: ${fullUrl}`);
    let response;
    try {
        response = await fetch(fullUrl, {
            headers: headers(token),
            method: method || "get",
            body: JSON.stringify(body),
        });
    }
    catch (ex) {
        return handleErrorFetch(ex);
    }
    if (response.ok) {
        var responseClone = response.clone();
        try {
            return await response.json();
        }
        catch (_a) {
            return await responseClone;
        }
    }
    else {
        logger.error("Response was not OK.");
        return await handleErrorResponse(response);
    }
};
