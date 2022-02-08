import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import GeneratorsSummary from "./MetricGeneratorsSummary";

const MetricGeneratorsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={GeneratorsSummary} />
      </Switch>
    </React.Fragment>
  );
};

export default MetricGeneratorsComponent;
