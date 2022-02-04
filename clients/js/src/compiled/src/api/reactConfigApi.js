import { getUrl } from "./client/baseUrl";
const defaultHeaders = { "Content-Type": "application/json" };
let authConfig = null; // caches this because it rarely change
export const fetchAuth0ConfigurationAsync = async () => {
    if (!authConfig) {
        console.log("fetching auth0 from server...");
        const result = await fetch(getUrl("api/reactConfig/auth0"), {
            headers: defaultHeaders,
        });
        authConfig = await result.json();
        console.log(authConfig);
    }
    return authConfig;
};
let config = null;
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
