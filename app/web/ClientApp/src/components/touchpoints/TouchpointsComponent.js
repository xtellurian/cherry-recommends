import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { TouchpointsSummary } from "./TouchpointsSummary";
import { CreateTouchpoint } from "./CreateTouchpoint";
import { UsersInTouchpoint } from "./UsersInTouchpoint";

export const TouchpointsComponent = () => {
  let { path } = useRouteMatch();
  return (
    <React.Fragment>
      <div>
        <Switch>
          <AuthorizeRoute
            exact
            path={`${path}`}
            component={TouchpointsSummary}
          />
          <AuthorizeRoute
            exact
            path={`${path}/create`}
            component={CreateTouchpoint}
          />
          <AuthorizeRoute
            exact
            path={`${path}/users-in-touchpoint/:id`}
            component={UsersInTouchpoint}
          />
        </Switch>
      </div>
    </React.Fragment>
  );
};
