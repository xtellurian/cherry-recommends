import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { ErrorBoundary } from "../../molecules/ErrorBoundary";
import { CreateEnvironment } from "./CreateEnvironment";
import { EnvironmentsSummary } from "./EnvironmentsSummary";

export const EnvironmentsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute
            exact
            path={`${path}`}
            component={EnvironmentsSummary}
          />
          <AuthorizeRoute
            exact
            path={`${path}/create`}
            component={CreateEnvironment}
          />
          {/* <AuthorizeRoute
          exact
          path={`${path}/reward-selector/:id`}
          component={RewardSelectorDetail}
        /> */}
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};
