import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { ParameterSetCampaignsSummary } from "./ParameterSetCampaignsSummary";
import { CreateParameterSetCampaign } from "./CreateParameterSetCampaign";
import { ParameterSetCampaignDetail } from "./ParameterSetCampaignDetail";
import { TestParameterSetCampaign } from "./TestParameterSetCampaign";
import { LinkToModel } from "./LinkToModel";
import { Overview } from "./Overview";
import { Settings } from "./Settings";
import { InvokationLogs } from "./InvokationLogs";
import { MonitorParameterSetCampaign } from "./MonitorParameterSetCampaign";
import { Destinations } from "./Destinations";
import { Triggers } from "./Triggers";
import { LearningMetrics } from "./LearningMetrics";
import { Arguments } from "./Arguments";
import { ErrorBoundary } from "../../molecules/ErrorBoundary";

export const ParameterSetCampaignsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute
            exact
            path={`${path}`}
            component={ParameterSetCampaignsSummary}
          />
          <AuthorizeRoute
            exact
            path={`${path}/create`}
            component={CreateParameterSetCampaign}
          />
          <AuthorizeRoute
            path={`${path}/detail/:id`}
            component={ParameterSetCampaignDetail}
          />
          <AuthorizeRoute path={`${path}/overview/:id`} component={Overview} />
          <AuthorizeRoute
            path={`${path}/test/:id`}
            component={TestParameterSetCampaign}
          />
          <AuthorizeRoute
            path={`${path}/link-to-model/:id`}
            component={LinkToModel}
          />
          <AuthorizeRoute
            path={`${path}/invokation-logs/:id`}
            component={InvokationLogs}
          />
          <AuthorizeRoute
            path={`${path}/monitor/:id`}
            component={MonitorParameterSetCampaign}
          />
          <AuthorizeRoute
            path={`${path}/destinations/:id`}
            component={Destinations}
          />
          <AuthorizeRoute
            path={`${path}/learning-metrics/:id`}
            component={LearningMetrics}
          />
          <AuthorizeRoute
            path={`${path}/arguments/:id`}
            component={Arguments}
          />
          <AuthorizeRoute path={`${path}/triggers/:id`} component={Triggers} />
          <AuthorizeRoute path={`${path}/settings/:id`} component={Settings} />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};
