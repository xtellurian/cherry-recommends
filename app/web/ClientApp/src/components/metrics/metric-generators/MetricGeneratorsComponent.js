import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { ErrorBoundary } from "../../molecules/ErrorBoundary";
import GeneratorsSummary from "./MetricGeneratorsSummary";

const MetricGeneratorsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute
            exact
            path={`${path}`}
            component={GeneratorsSummary}
          />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};

export default MetricGeneratorsComponent;
