import { getUrl } from "./client/baseUrl";
const defaultHeaders = { "Content-Type": "application/json" };
let config = null; // caches this because it rarely change
export const fetchAuth0ConfigurationAsync = async () => {
    if (!config) {
        console.log("fetching auth0 from server...");
        const result = await fetch(getUrl("api/reactConfig/auth0"), {
            headers: defaultHeaders,
        });
        config = await result.json();
        console.log(config);
    }
    return config;
};
