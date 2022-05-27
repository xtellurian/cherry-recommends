import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";

import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { PromotionsCampaignsSummary } from "./PromotionsCampaignsSummary";
import { CreateCampaign } from "./CreatePromotionsCampaign";
import { CampaignDetail } from "./PromotionsCampaignDetail";
import { TestCampaign } from "./TestPromotionsCampaign";
import { LinkToModel } from "./LinkToModel";
import { TargetVariableValues } from "./TargetVariableValues";
import { InvokationLogs } from "./InvokationLogs";
import { SettingsPage } from "./SettingsPage";
import { MonitorCampaign } from "./MonitorPromotionsCampaign";
import { ManageItems } from "./manage/ManagePromotions";
import { ManageComponent } from "./manage/ManageComponent";
import { Destinations } from "./Destinations";
import { Triggers } from "./Triggers";
import { LearningMetrics } from "./LearningMetrics";
import { Arguments } from "./Arguments";
import { Overview } from "./Overview";
import { Delivery } from "./Delivery";
import Performance from "./PerformanceTable";
import { ErrorBoundary } from "../../molecules/ErrorBoundary";

export const PromotionsCampaignsComponent = () => {
  const { path } = useRouteMatch();

  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute
            exact
            path={`${path}`}
            component={PromotionsCampaignsSummary}
          />
          <AuthorizeRoute
            exact
            path={`${path}/create`}
            component={CreateCampaign}
          />
          <AuthorizeRoute
            exact
            path={`${path}/overview/:id`}
            component={Overview}
          />
          <AuthorizeRoute
            exact
            path={`${path}/detail/:id`}
            component={CampaignDetail}
          />
          <AuthorizeRoute
            path={`${path}/link-to-model/:id`}
            component={LinkToModel}
          />
          <AuthorizeRoute path={`${path}/test/:id`} component={TestCampaign} />
          <AuthorizeRoute
            path={`${path}/target-variable/:id`}
            component={TargetVariableValues}
          />
          <AuthorizeRoute
            path={`${path}/invokation-logs/:id`}
            component={InvokationLogs}
          />
          <AuthorizeRoute
            path={`${path}/monitor/:id`}
            component={MonitorCampaign}
          />
          <AuthorizeRoute
            path={`${path}/manage-promotions/:id`}
            component={ManageItems}
          />
          <AuthorizeRoute path={`${path}/manage`} component={ManageComponent} />
          <AuthorizeRoute path={`${path}/triggers/:id`} component={Triggers} />
          <AuthorizeRoute
            path={`${path}/settings/:id`}
            component={SettingsPage}
          />
          <AuthorizeRoute
            path={`${path}/learning-metrics/:id`}
            component={LearningMetrics}
          />
          <AuthorizeRoute
            path={`${path}/arguments/:id`}
            component={Arguments}
          />
          <AuthorizeRoute
            path={`${path}/reports/:id`}
            component={Performance}
          />
          <AuthorizeRoute path={`${path}/delivery/:id`} component={Delivery} />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};
