import { executeFetch } from "./client/apiClientTs";
export const fetchApiKeysAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/apiKeys",
        token,
        page,
    });
};
var ApiKeyType;
(function (ApiKeyType) {
    ApiKeyType["Server"] = "Server";
    ApiKeyType["Web"] = "Web";
})(ApiKeyType || (ApiKeyType = {}));
export const createApiKeyAsync = async ({ token, payload, }) => {
    return await executeFetch({
        path: "api/apiKeys",
        token,
        method: "post",
        body: payload,
    });
};
export const exchangeApiKeyAsync = async ({ apiKey, }) => {
    return await executeFetch({
        path: "api/apiKeys/exchange",
        method: "post",
        body: { apiKey },
    });
};
export const deleteApiKeyAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/apiKeys/${id}`,
        token,
        method: "delete",
    });
};
