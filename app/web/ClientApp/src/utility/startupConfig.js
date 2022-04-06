import {
  fetchAuth0ConfigurationAsync,
  fetchHostingAsync,
} from "../api/reactConfigApi";
import { getLDProviderAsync } from "./launchDarkly";

export const getStartupConfigAsync = async () => {
  const hosting = await fetchHostingAsync();
  const auth0 = await fetchAuth0ConfigurationAsync();
  const LDProvider = await getLDProviderAsync();
  return {
    auth0,
    LDProvider,
    hosting,
  };
};
