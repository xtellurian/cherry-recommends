import React, { useMemo } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { useAuth0Config } from "./reactConfigApi";
import { decode } from "jsonwebtoken";

export const useAccessToken = () => {
  const { getAccessTokenSilently, isAuthenticated } = useAuth0();
  const config = useAuth0Config();
  const [accessToken, setAccessToken] = React.useState();

  React.useEffect(() => {
    if (isAuthenticated && config) {
      getAccessTokenSilently({
        audience: config.audience,
        scope: "read:current_user",
      })
        .then(setAccessToken)
        .catch(console.log);
    }
  }, [isAuthenticated, config, getAccessTokenSilently]);

  return accessToken;
};

export const useTokenScopes = () => {
  const token = useAccessToken();
  const scopes = useMemo(() => {
    const decoded = decode(token);
    return decoded ? decoded.scope.split(" ") : [];
  }, [token]);
  return scopes;
};

export const useAuth0AccessToken = () => {
  const { getAccessTokenSilently, isAuthenticated } = useAuth0();
  const config = useAuth0Config();
  const [accessToken, setAccessToken] = React.useState();

  React.useEffect(() => {
    if (isAuthenticated && config && config.managementAudience) {
      getAccessTokenSilently({
        audience: config.managementAudience,
        scope: "read:current_user_metadata",
      })
        .then(setAccessToken)
        .catch(console.log);
    } else {
      console.log(config);
    }
  }, [isAuthenticated, config, getAccessTokenSilently]);

  return accessToken;
};
