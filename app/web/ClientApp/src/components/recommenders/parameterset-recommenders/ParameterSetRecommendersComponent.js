import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { ParameterSetRecommendersSummary } from "./ParameterSetRecommendersSummary";
import { CreateParameterSetRecommender } from "./CreateParameterSetRecommender";
import { ParameterSetRecommenderDetail } from "./ParameterSetRecommenderDetail";
import { TestParameterSetRecommender } from "./TestParameterSetRecommender";
import { IntegrateParameterSetRecommender } from "./IntegrateParameterSetRecommender";
import { RecommendationList } from "./RecommendationList";
import { LinkToModel } from "./LinkToModel";
import { TargetVariableValues } from "./TargetVariableValues";

export const ParameterSetRecommendersComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
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
        <AuthorizeRoute
          path={`${path}/test/:id`}
          component={TestParameterSetRecommender}
        />
        <AuthorizeRoute
          path={`${path}/link-to-model/:id`}
          component={LinkToModel}
        />
        <AuthorizeRoute
          path={`${path}/recommendations/:id`}
          component={RecommendationList}
        />
        <AuthorizeRoute
          path={`${path}/integrate/:id`}
          component={IntegrateParameterSetRecommender}
        />
        <AuthorizeRoute
          path={`${path}/target-variable/:id`}
          component={TargetVariableValues}
        />
      </Switch>
    </React.Fragment>
  );
};
