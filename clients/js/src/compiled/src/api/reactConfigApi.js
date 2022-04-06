import { executeFetch } from "./client/apiClient";
let authConfig = undefined; // caches this because it rarely change
export const fetchAuth0ConfigurationAsync = async () => {
    if (!authConfig) {
        const result = await executeFetch({
            path: "api/reactConfig/auth0",
        });
        authConfig = result;
    }
    return authConfig;
};
let config = undefined;
export const fetchConfigurationAsync = async () => {
    if (!config) {
        const result = await executeFetch({
            token: "",
            path: "api/reactConfig",
        });
        config = result;
    }
    return config;
};
export const fetchHostingAsync = async () => {
    return await executeFetch({
        path: "api/reactConfig/hosting",
        method: "get",
    });
};
