import { getUrl } from "./client/baseUrl";
import { components } from "../model/api";
import { current } from "./client/axiosInstance";

const defaultHeaders = { "Content-Type": "application/json" };

type Auth0ReactConfig = components["schemas"]["Auth0ReactConfig"] | undefined;
let authConfig: Auth0ReactConfig = undefined; // caches this because it rarely change
export const fetchAuth0ConfigurationAsync =
  async (): Promise<Auth0ReactConfig> => {
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

type ReactConfig = components["schemas"]["ReactConfig"] | undefined;
let config: ReactConfig = undefined;
export const fetchConfigurationAsync = async (): Promise<ReactConfig> => {
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
