import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ErrorBoundary } from "../molecules/ErrorBoundary";
import { ParameterSetCampaignsComponent } from "./parameterset-campaigns/ParameterSetCampaignsComponent";
import { PromotionsCampaignsComponent } from "./promotions-campaigns/PromotionsCampaignsComponent";

export const CampaignsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute
            path={`${path}/parameter-set-campaigns`}
            component={ParameterSetCampaignsComponent}
          />
          <AuthorizeRoute
            path={`${path}/promotions-campaigns`}
            component={PromotionsCampaignsComponent}
          />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};
