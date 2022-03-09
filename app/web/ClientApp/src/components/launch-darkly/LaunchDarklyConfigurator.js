import { useAuth0 } from "@auth0/auth0-react";
import { useLDClient } from "launchdarkly-react-client-sdk";
import React from "react";

const LaunchDarklyConfigurator = ({ children }) => {
  const { isAuthenticated, user } = useAuth0();
  const ld = useLDClient();
  if (isAuthenticated && user && ld) {
    ld.identify({
      key: user.sub,
      email: user.email,
      name: user.name,
      firstName: user.given_name,
    });
  }
  return <React.Fragment>{children}</React.Fragment>;
};

export default LaunchDarklyConfigurator;
