import React from "react";
import { Route, Switch, useRouteMatch } from "react-router-dom";
import { ShopifyConnector } from "../settings/integrations/shopify/ShopifyConnector";

export const ConnectComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <Route
          exact
          path={`${path}/shopify/callback`}
          component={ShopifyConnector}
        />
      </Switch>
    </React.Fragment>
  );
};
