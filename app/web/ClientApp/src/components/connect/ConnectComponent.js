import React from "react";
import { Route, Switch, useRouteMatch } from "react-router-dom";
import { ShopifyConnector } from "../settings/integrations/shopify/ShopifyConnector";
import { ShopifyInstall } from "../settings/integrations/shopify/ShopifyInstall";
import { ErrorBoundary } from "../molecules/ErrorBoundary";

export const ConnectComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <Route
            exact
            path={`${path}/shopify/callback`}
            component={ShopifyConnector}
          />
          <Route
            exact
            path={`${path}/shopify/install`}
            component={ShopifyInstall}
          />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};
