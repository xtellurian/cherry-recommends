import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ErrorBoundary } from "../molecules/ErrorBoundary";
import { EventDetailPage } from "./EventDetail";

export const EventsComponent = () => {
  let { path } = useRouteMatch();

  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute
            exact
            path={`${path}/detail/:id`}
            component={EventDetailPage}
          />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};
