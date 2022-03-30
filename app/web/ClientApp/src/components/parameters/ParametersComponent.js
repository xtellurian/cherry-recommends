import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ParametersSummary } from "./ParametersSummary";
import { CreateParameter } from "./CreateParameter";

export const ParametersComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={ParametersSummary} />
        <AuthorizeRoute
          exact
          path={`${path}/create`}
          component={CreateParameter}
        />
      </Switch>
    </React.Fragment>
  );
};
