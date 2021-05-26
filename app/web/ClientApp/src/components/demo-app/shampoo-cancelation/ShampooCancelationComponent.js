import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { ChoosePersonaComponent } from "./ChoosePersonaComponent";
import { CancelSubscription } from "./CancelSubscription";

export const ShampooCancelationComponent = () => {
  let { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute
          exact
          path={`${path}`}
          component={ChoosePersonaComponent}
        />
        <AuthorizeRoute
          exact
          path={`${path}/cancel`}
          component={CancelSubscription}
        />
      </Switch>
    </React.Fragment>
  );
};
