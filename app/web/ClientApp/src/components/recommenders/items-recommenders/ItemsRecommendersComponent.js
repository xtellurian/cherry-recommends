import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { ItemsRecommendersSummary } from "./ItemsRecommendersSummary";
import { CreateRecommender } from "./CreateItemsRecommender";
import { RecommenderDetail } from "./ItemsRecommenderDetail";
import { TestRecommender } from "./TestItemsRecommender";
import { LinkToModel } from "./LinkToModel";
import { TargetVariableValues } from "./TargetVariableValues";
import { InvokationLogs } from "./InvokationLogs";
import { Settings } from "./Settings";
import { MonitorRecommender } from "./MonitorItemsRecommender";
import { ManageItems } from "./ManageItems";
import { Destinations } from "./Destinations";
import { Triggers } from "./Triggers";
import { LearningMetrics } from "./LearningMetrics";
import { Arguments } from "./Arguments";
import { Overview } from "./Overview";
import Performance from "./ItemRecommenderPerformance";

export const ItemsRecommendersComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute
          exact
          path={`${path}`}
          component={ItemsRecommendersSummary}
        />
        <AuthorizeRoute
          exact
          path={`${path}/create`}
          component={CreateRecommender}
        />
        <AuthorizeRoute
          exact
          path={`${path}/overview/:id`}
          component={Overview}
        />
        <AuthorizeRoute
          exact
          path={`${path}/detail/:id`}
          component={RecommenderDetail}
        />
        <AuthorizeRoute
          path={`${path}/link-to-model/:id`}
          component={LinkToModel}
        />
        <AuthorizeRoute path={`${path}/test/:id`} component={TestRecommender} />
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
          component={MonitorRecommender}
        />
        <AuthorizeRoute
          path={`${path}/manage-items/:id`}
          component={ManageItems}
        />
        <AuthorizeRoute
          path={`${path}/destinations/:id`}
          component={Destinations}
        />
        <AuthorizeRoute path={`${path}/triggers/:id`} component={Triggers} />
        <AuthorizeRoute path={`${path}/settings/:id`} component={Settings} />
        <AuthorizeRoute
          path={`${path}/learning-metrics/:id`}
          component={LearningMetrics}
        />
        <AuthorizeRoute path={`${path}/arguments/:id`} component={Arguments} />
        <AuthorizeRoute
          path={`${path}/performance/:id`}
          component={Performance}
        />
      </Switch>
    </React.Fragment>
  );
};
