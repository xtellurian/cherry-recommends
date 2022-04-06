import { current } from "./client/axiosInstance";
import { executeFetch } from "./client/apiClient";
const defaultHeaders = { "Content-Type": "application/json" };
let authConfig = undefined; // caches this because it rarely change
export const fetchAuth0ConfigurationAsync = async () => {
    if (!authConfig) {
        console.debug("fetching auth0 from server...");
        const axios = current();
        const result = await axios.get("api/reactConfig/auth0", {
            headers: defaultHeaders,
        });
        authConfig = result.data;
        console.log("authConfig");
        console.log(authConfig);
    }
    return authConfig;
};
let config = undefined;
export const fetchConfigurationAsync = async () => {
    if (!config) {
        console.log("fetching configuration from server...");
        const axios = current();
        const result = await axios.get("api/reactConfig", {
            headers: defaultHeaders,
        });
        config = result.data;
        console.log("config");
        console.log(config);
    }
    return config;
};
export const fetchHostingAsync = async () => {
    return await executeFetch({
        path: "api/reactConfig/hosting",
        method: "get",
    });
};
