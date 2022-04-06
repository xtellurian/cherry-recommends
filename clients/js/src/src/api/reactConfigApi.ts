import { components } from "../model/api";
import { executeFetch } from "./client/apiClient";

type Auth0ReactConfig = components["schemas"]["Auth0ReactConfig"] | undefined;
let authConfig: Auth0ReactConfig = undefined; // caches this because it rarely change
export const fetchAuth0ConfigurationAsync =
  async (): Promise<Auth0ReactConfig> => {
    if (!authConfig) {
      const result = await executeFetch({
        path: "api/reactConfig/auth0",
      });
      authConfig = result;
    }
    return authConfig;
  };

type ReactConfig = components["schemas"]["ReactConfig"] | undefined;
let config: ReactConfig = undefined;
export const fetchConfigurationAsync = async (): Promise<ReactConfig> => {
  if (!config) {
    const result = await executeFetch({
      token: "",
      path: "api/reactConfig",
    });

    config = result;
  }
  return config;
};

type Hosting = components["schemas"]["Hosting"];
export const fetchHostingAsync = async (): Promise<Hosting> => {
  return await executeFetch({
    path: "api/reactConfig/hosting",
    method: "get",
  });
};
