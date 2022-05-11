import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { ParameterSetRecommendersSummary } from "./ParameterSetRecommendersSummary";
import { CreateParameterSetRecommender } from "./CreateParameterSetRecommender";
import { ParameterSetRecommenderDetail } from "./ParameterSetRecommenderDetail";
import { TestParameterSetRecommender } from "./TestParameterSetRecommender";
import { LinkToModel } from "./LinkToModel";
import { Overview } from "./Overview";
import { Settings } from "./Settings";
import { InvokationLogs } from "./InvokationLogs";
import { MonitorParameterSetRecommender } from "./MonitorParameterSetRecommender";
import { Destinations } from "./Destinations";
import { Triggers } from "./Triggers";
import { LearningMetrics } from "./LearningMetrics";
import { Arguments } from "./Arguments";
import { ErrorBoundary } from "../../molecules/ErrorBoundary";

export const ParameterSetRecommendersComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute
            exact
            path={`${path}`}
            component={ParameterSetRecommendersSummary}
          />
          <AuthorizeRoute
            exact
            path={`${path}/create`}
            component={CreateParameterSetRecommender}
          />
          <AuthorizeRoute
            path={`${path}/detail/:id`}
            component={ParameterSetRecommenderDetail}
          />
          <AuthorizeRoute path={`${path}/overview/:id`} component={Overview} />
          <AuthorizeRoute
            path={`${path}/test/:id`}
            component={TestParameterSetRecommender}
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
            component={MonitorParameterSetRecommender}
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
