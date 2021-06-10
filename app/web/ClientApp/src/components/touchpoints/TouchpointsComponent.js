import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { TouchpointsSummary } from "./TouchpointsSummary";
import { CreateTouchpoint } from "./CreateTouchpoint";

export const TouchpointsComponent = (props) => {
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
        </Switch>
      </div>
    </React.Fragment>
  );
};
