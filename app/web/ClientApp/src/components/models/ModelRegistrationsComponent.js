import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { CreateModelRegistration } from "./CreateModelRegistration";
import { ModelRegistrationsSummary } from "./ModelRegistrationsSummary";
import { TestModel } from "./TestModel";

export const ModelRegistrationsComponent = () => {
  let { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute
          exact
          path={`${path}`}
          component={ModelRegistrationsSummary}
        />
        <AuthorizeRoute
          exact
          path={`${path}/create`}
          component={CreateModelRegistration}
        />
        <AuthorizeRoute exact path={`${path}/test/:id`} component={TestModel} />
      </Switch>
    </React.Fragment>
  );
};
