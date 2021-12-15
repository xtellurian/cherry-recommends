let defaltEnvironmentId = null;
export const setDefaultEnvironmentId = (e) => {
    defaltEnvironmentId = e;
};
let defaultApiKey = null;
export const setDefaultApiKey = (k) => {
    defaultApiKey = k;
};
export const defaultHeaders = { "Content-Type": "application/json" };
export const headers = (token, apiKey) => {
    let headers = { ...defaultHeaders };
    if (token) {
        headers.Authorization = `Bearer ${token}`;
    }
    if (apiKey) {
        headers["x-api-key"] = `${apiKey}`; // ensure its a string
    }
    else if (defaultApiKey) {
        headers["x-api-key"] = `${defaultApiKey}`; // ensure its a string
    }
    if (defaltEnvironmentId) {
        headers["x-environment"] = defaltEnvironmentId;
    }
    return headers;
};
