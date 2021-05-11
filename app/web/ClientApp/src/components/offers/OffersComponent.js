import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute"
import { OffersSummary } from "./OffersSummary";
import { CreateOffer } from "./CreateOffer";
import { OfferDetail } from "./OfferDetail";

export const OffersComponent = () => {
  let { path } = useRouteMatch();

  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={OffersSummary} />
        <AuthorizeRoute exact path={`${path}/create`} component={CreateOffer} />
        <AuthorizeRoute path={`${path}/:id`} component={OfferDetail} />
      </Switch>
    </React.Fragment>
  );
};
