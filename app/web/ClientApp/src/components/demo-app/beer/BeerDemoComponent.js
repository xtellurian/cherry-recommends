import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { BeerSubscription } from "./BeerSubscription";
import { ChooseState } from "./ChooseState";
import { BeerSubscriptionConfirmation } from "./BeerSubscriptionConfirmation";

export const BeerDemoComponent = () => {
  let { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={ChooseState} />
        <AuthorizeRoute exact path={`${path}/subscribe`} component={BeerSubscription} />
        <AuthorizeRoute
          path={`${path}/subscribe/confirm`}
          component={BeerSubscriptionConfirmation}
        />
      </Switch>
    </React.Fragment>
  );
};
