import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ChannelsSummary } from "./ChannelsSummary";
import { CreateChannel } from "./CreateChannel";

export const ChannelsComponent = (props) => {
  let { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={ChannelsSummary} />
        <AuthorizeRoute
          exact
          path={`${path}/create`}
          component={CreateChannel}
        />
      </Switch>
    </React.Fragment>
  );
};
