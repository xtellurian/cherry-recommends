import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ManagementPage } from "./managementPage";
import { CreateTenantSection } from "./CreateTenantSection";
import { ErrorBoundary } from "../molecules/ErrorBoundary";

export const TenantsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute exact path={`${path}`} component={ManagementPage} />
          <AuthorizeRoute
            exact
            path={`${path}/create-tenant`}
            component={CreateTenantSection}
          />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};
