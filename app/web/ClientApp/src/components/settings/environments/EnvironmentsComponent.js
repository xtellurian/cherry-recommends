import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { CreateEnvironment } from "./CreateEnvironment";
import { EnvironmentsSummary } from "./EnvironmentsSummary";

export const EnvironmentsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={EnvironmentsSummary} />
        <AuthorizeRoute
          exact
          path={`${path}/create`}
          component={CreateEnvironment}
        />
        {/* <AuthorizeRoute
          exact
          path={`${path}/reward-selector/:id`}
          component={RewardSelectorDetail}
        /> */}
      </Switch>
    </React.Fragment>
  );
};
