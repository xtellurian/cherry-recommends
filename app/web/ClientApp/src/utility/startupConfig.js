import { fetchAuth0ConfigurationAsync } from "../api/reactConfigApi";
import { getLDProviderAsync } from "./launchDarkly";

export const getStartupConfigAsync = async () => {
  const auth0 = await fetchAuth0ConfigurationAsync();
  const LDProvider = await getLDProviderAsync();
  return {
    auth0,
    LDProvider,
  };
};
