import React from "react";
import { useAccessToken, useAuth0AccessToken } from "./token";
import { loadingState } from "./loadingState";
import { useAuth0Config } from "./reactConfigApi";
import { useAuth0 } from "@auth0/auth0-react";
import { getMetadataAsync } from "../api/profileApi";

export const useMetadataDirectly = (props) => {
  const config = useAuth0Config();
  const { user, isAuthenticated } = useAuth0();
  const [userMetadata, setUserMetadata] = React.useState(loadingState);
  const token = useAuth0AccessToken();

  React.useEffect(() => {
    if (token && user && config && config.domain && isAuthenticated) {
      const userDetailsByIdUrl = `https://${config.domain}/api/v2/users/${user.sub}`;
      fetch(userDetailsByIdUrl, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
        .then((m) => m.json())
        .then((res) => setUserMetadata(res.user_metadata))
        .catch((error) => setUserMetadata({ error }));
    }
  }, [token, user, config, props.trigger]);

  return userMetadata;
};

export const useMetadata = (props) => {
  const token = useAccessToken();
  const [state, setState] = React.useState(loadingState);

  React.useEffect(() => {
    if (token) {
      setState(loadingState);
      getMetadataAsync({ token })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, props.trigger]);

  return state;
};
