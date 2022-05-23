import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";

import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ErrorBoundary } from "../molecules/ErrorBoundary";
import { ModelRegistrationsComponent } from "../models/ModelRegistrationsComponent";

export const AdminComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <div className="border border-warning rounded text-muted text-center mb-4">
        Admin Zone
      </div>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute
            path={`${path}/models`}
            component={ModelRegistrationsComponent}
          />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};
