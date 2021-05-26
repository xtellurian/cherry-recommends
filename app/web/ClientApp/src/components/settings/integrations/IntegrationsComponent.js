import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { ListIntegrations } from "./ListIntegrations";
import { CreateIntegration } from "./CreateIntegration";

export const IntegrationsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={ListIntegrations} />
        <AuthorizeRoute
          exact
          path={`${path}/create`}
          component={CreateIntegration}
        />
      </Switch>
    </React.Fragment>
  );
};
