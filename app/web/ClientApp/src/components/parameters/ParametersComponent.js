import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ParametersSummary } from "./ParametersSummary";
import { CreateParameter } from "./CreateParameter";
import { ErrorBoundary } from "../molecules/ErrorBoundary";

const DefaultComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute
            exact
            path={`${path}`}
            component={ParametersSummary}
          />
          <AuthorizeRoute
            exact
            path={`${path}/create`}
            component={CreateParameter}
          />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};

export const ParametersComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute
            path={`${path}/parameters`}
            component={DefaultComponent}
          />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};
