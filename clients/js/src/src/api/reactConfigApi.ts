import { getUrl } from "./client/baseUrl";
import { components } from "../model/api";
import fetch from "./client/fetchWrapper";

const defaultHeaders = { "Content-Type": "application/json" };

type Auth0ReactConfig = components["schemas"]["Auth0ReactConfig"] | undefined;
let authConfig: Auth0ReactConfig = undefined; // caches this because it rarely change
export const fetchAuth0ConfigurationAsync =
  async (): Promise<Auth0ReactConfig> => {
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

type ReactConfig = components["schemas"]["ReactConfig"] | undefined;
let config: ReactConfig = undefined;
export const fetchConfigurationAsync = async (): Promise<ReactConfig> => {
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
