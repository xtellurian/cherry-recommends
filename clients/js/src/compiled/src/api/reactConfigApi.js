import { getUrl } from "./client/baseUrl";
import fetch from "./client/fetchWrapper";
const defaultHeaders = { "Content-Type": "application/json" };
let authConfig = undefined; // caches this because it rarely change
export const fetchAuth0ConfigurationAsync = async () => {
    if (!authConfig) {
        console.debug("fetching auth0 from server...");
        const result = await fetch(getUrl("api/reactConfig/auth0"), {
            headers: defaultHeaders,
        });
        authConfig = await result.json();
        console.log(authConfig);
    }
    return authConfig;
};
let config = undefined;
export const fetchConfigurationAsync = async () => {
    if (!config) {
        console.log("fetching configuration from server...");
        const result = await fetch(getUrl("api/reactConfig"), {
            headers: defaultHeaders,
        });
        config = await result.json();
        console.log(config);
    }
    return config;
};
