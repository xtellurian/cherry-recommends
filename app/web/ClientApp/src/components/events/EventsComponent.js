import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { EventDetail } from "./EventDetail";

export const EventsComponent = () => {
  let { path } = useRouteMatch();

  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute
          exact
          path={`${path}/detail/:id`}
          component={EventDetail}
        />
      </Switch>
    </React.Fragment>
  );
};
