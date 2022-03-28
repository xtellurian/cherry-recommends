import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../../auth0/ProtectedRoute";
import Weights from "./ManageWeights";
import { ManageItems } from "./ManagePromotions";

export const ManageComponent = () => {
  const { path } = useRouteMatch();
  return (
    <Switch>
      <AuthorizeRoute
        exact
        path={`${path}/promotions/:id`}
        component={ManageItems}
      />
      <AuthorizeRoute exact path={`${path}/weights/:id`} component={Weights} />
    </Switch>
  );
};
