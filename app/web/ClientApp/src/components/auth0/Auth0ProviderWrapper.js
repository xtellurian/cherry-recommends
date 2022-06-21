import React from "react";
import { useNavigate } from "react-router-dom";
import { Auth0Provider } from "@auth0/auth0-react";

export const specialPaths = [
  "/_connect/shopify/callback",
  "/_connect/shopify/install",
  "/_connect/hubspot/callback",
];

export const isSpecialPath = (path) => {
  const targetPath = path ?? window.location.pathname;
  // Use includes to match multi-tenant paths
  var match = specialPaths.find((_) => targetPath.includes(_));
  return !!match;
};

export const Auth0ProviderWrapper = ({ auth0Config, children }) => {
  // TODO: make this work for the new multi-tenant callbacks
  const navigate = useNavigate();

  const onRedirectCallback = (appState) => {
    navigate(
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
