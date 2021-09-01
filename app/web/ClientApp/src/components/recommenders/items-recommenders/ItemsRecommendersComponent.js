import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { ItemsRecommendersSummary } from "./ItemsRecommendersSummary";
import { CreateRecommender } from "./CreateItemsRecommender";
import { RecommenderDetail } from "./ItemsRecommenderDetail";
import { TestRecommender } from "./TestItemsRecommender";
import { RecommendationList } from "./RecommendationList";
import { IntegrateItemsRecommender } from "./IntegrateItemsRecommender";
import { LinkToModel } from "./LinkToModel";
import { TargetVariableValues } from "./TargetVariableValues";
import { InvokationLogs } from "./InvokationLogs";
import { Settings } from "./Settings";
import { MonitorRecommender } from "./MonitorItemsRecommender";

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
          path={`${path}/detail/:id`}
          component={RecommenderDetail}
        />
        <AuthorizeRoute
          path={`${path}/link-to-model/:id`}
          component={LinkToModel}
        />
        <AuthorizeRoute
          path={`${path}/test/:id`}
          component={TestRecommender}
        />
        <AuthorizeRoute
          path={`${path}/recommendations/:id`}
          component={RecommendationList}
        />
        <AuthorizeRoute
          path={`${path}/integrate/:id`}
          component={IntegrateItemsRecommender}
        />
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
        <AuthorizeRoute path={`${path}/settings/:id`} component={Settings} />
      </Switch>
    </React.Fragment>
  );
};
