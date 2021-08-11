import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { RewardsSummary } from "./RewardsSummary";
import { CreateRewardSelector } from "./CreateRewardSelector";
import { RewardSelectorDetail } from "./RewardSelectorDetail";

export const RewardsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={RewardsSummary} />
        <AuthorizeRoute
          exact
          path={`${path}/create-selector`}
          component={CreateRewardSelector}
        />
        <AuthorizeRoute
          exact
          path={`${path}/reward-selector/:id`}
          component={RewardSelectorDetail}
        />
      </Switch>
    </React.Fragment>
  );
};
