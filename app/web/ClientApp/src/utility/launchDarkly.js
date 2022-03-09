import { asyncWithLDProvider } from "launchdarkly-react-client-sdk";
import React from "react";
import { fetchConfigurationAsync } from "../api/reactConfigApi";
// do not enable secure mode for Launch Darkly
export const getLDProviderAsync = async () => {
  const { launchDarkly } = await fetchConfigurationAsync();
  if (launchDarkly.clientSideId) {
    return await asyncWithLDProvider({
      // clientSideID: "622802e7cc6fc7149d5348fd", // dev key
      clientSideID: launchDarkly.clientSideId, // dev key
      user: {
        anonymous: true,
      },
      options: {},
    });
  } else {
    return React.Fragment;
  }
};
