import React from "react";
import { useRouteMatch, Link, Switch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { Title } from "../molecules/PageHeadings";
import { ViewEventData } from "./ViewEventData";

const DataViewHome = () => {
  return (
    <div>
      <Title>Data View</Title>
      <hr />
      <Link to="dataview/events">
        <button className="btn btn-primary">Event Data</button>
      </Link>
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
