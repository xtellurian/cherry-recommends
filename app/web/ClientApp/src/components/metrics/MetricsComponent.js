import React from "react";
import { Redirect, Route } from "react-router";
import { Switch, useRouteMatch } from "react-router-dom";

import AuthorizeRoute from "../auth0/ProtectedRoute";
import MetricsSummary from "./MetricsSummary";
import Create from "./CreateMetric";
import Detail from "./MetricDetail";
import SetMetricValue from "./SetMetricValue";
import GeneratorsComponent from "./metric-generators/MetricGeneratorsComponent";
import { ErrorBoundary } from "../molecules/ErrorBoundary";

const DefaultComponent = () => {
  const { path } = useRouteMatch();

  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute exact path={`${path}`} component={MetricsSummary} />
          <AuthorizeRoute exact path={`${path}/create`} component={Create} />
          <AuthorizeRoute
            exact
            path={`${path}/detail/:id`}
            component={Detail}
          />
          <AuthorizeRoute
            exact
            path={`${path}/set-value/:id`}
            component={SetMetricValue}
          />
          <AuthorizeRoute
            exact
            path={`${path}/generators`}
            component={GeneratorsComponent}
          />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};

const MetricsComponent = () => {
  const { path } = useRouteMatch();

  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute
            path={`${path}/metrics`}
            component={DefaultComponent}
          />
          <Route path={path}>
            <Redirect to={`${path}/metrics`} />
          </Route>
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};

export default MetricsComponent;
