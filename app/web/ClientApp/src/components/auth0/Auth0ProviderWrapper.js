import { Auth0Provider } from "@auth0/auth0-react";
import React from "react";
import { useHistory } from "react-router-dom";

export const specialPaths = [
  "/settings/integrations/hubspotconnector",
  "/settings/integrations/detail",
  "/_connect/shopify/callback",
  "/_connect/shopify/install",
];

export const isSpecialPath = (path) => {
  const targetPath = path ?? window.location.pathname;
  // Use includes to match multi-tenant paths
  var match = specialPaths.find((_) => targetPath.includes(_));
  return !!match;
};

export const Auth0ProviderWrapper = ({ auth0Config, children }) => {
  // TODO: make this work for the new multi-tenant callbacks
  const history = useHistory();
  const onRedirectCallback = (appState) => {
    history.push(
      appState && appState.returnTo
        ? appState.returnTo
        : window.location.pathname
    );
  };

  return (
    <Auth0Provider
      domain={auth0Config.domain}
      clientId={auth0Config.clientId}
      redirectUri={auth0Config.redirectUri || window.location.origin}
      audience={auth0Config.audience}
      scope={auth0Config.scope}
      skipRedirectCallback={
        // this is a boolean, not just the one path
        isSpecialPath()
      }
      onRedirectCallback={onRedirectCallback}
    >
      {children}
    </Auth0Provider>
  );
};
