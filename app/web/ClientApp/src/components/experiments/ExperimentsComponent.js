import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { CreateExperiment } from "./CreateExperiment";
import { ExperimentResults } from "./ExperimentResults";
import { ExperimentsSummary } from "./ExperimentsSummary";

export const ExperimentsComponent = () => {
  let { path } = useRouteMatch();

  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={ExperimentsSummary} />
        <AuthorizeRoute
          exact
          path={`${path}/create`}
          component={CreateExperiment}
        />
        <AuthorizeRoute
          exact
          path={`${path}/create`}
          component={CreateExperiment}
        />
        <AuthorizeRoute
          exact
          path={`${path}/results/:id`}
          component={ExperimentResults}
        />
      </Switch>
    </React.Fragment>
  );
};
