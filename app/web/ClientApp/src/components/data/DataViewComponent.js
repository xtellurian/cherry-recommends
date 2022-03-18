import React from "react";
import { useRouteMatch, Switch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { Title } from "../molecules/layout";
import { ViewEventData } from "./ViewEventData";

const DataViewHome = () => {
  return (
    <div>
      <Title>Events Overview</Title>
      <hr />
      <ViewEventData />
    </div>
  );
};

export const DataViewComponent = () => {
  const { path } = useRouteMatch();

  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={DataViewHome} />
        <AuthorizeRoute
          exact
          path={`${path}/events`}
          component={ViewEventData}
        />
      </Switch>
    </React.Fragment>
  );
};
