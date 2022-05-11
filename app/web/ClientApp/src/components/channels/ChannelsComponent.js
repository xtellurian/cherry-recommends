import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ChannelsSummary } from "./ChannelsSummary";
import { CreateChannel } from "./CreateChannel";
import { ChannelDetail } from "./ChannelDetail";
import { ErrorBoundary } from "../molecules/ErrorBoundary";

export const ChannelsComponent = (props) => {
  let { path } = useRouteMatch();
  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute exact path={`${path}`} component={ChannelsSummary} />
          <AuthorizeRoute
            exact
            path={`${path}/create`}
            component={CreateChannel}
          />
          <AuthorizeRoute
            exact
            path={`${path}/detail/:id`}
            component={ChannelDetail}
          />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};
