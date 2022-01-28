import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import MetricsSummary from "./MetricsSummary";
import Create from "./CreateMetric";
import Detail from "./MetricDetail";
import SetMetricValue from "./SetMetricValue";
import GeneratorsComponent from "./metric-generators/MetricGeneratorsComponent";

const MetricsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
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
    </React.Fragment>
  );
};

export default MetricsComponent;
