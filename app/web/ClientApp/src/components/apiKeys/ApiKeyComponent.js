import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ListApiKeys } from "./ListApiKeys";
import { CreateApiKey } from "./CreateApiKey";


export const ApiKeyComponent = () => {
  let { path } = useRouteMatch();

  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={ListApiKeys} />
        <AuthorizeRoute exact path={`${path}/create`} component={CreateApiKey} />
      </Switch>
    </React.Fragment>
  );
};
