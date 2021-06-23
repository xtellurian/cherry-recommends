import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { ParameterSetRecommendersSummary } from "./ParameterSetRecommendersSummary";
import { CreateParameterSetRecommender } from "./CreateParameterSetRecommender";
import { ParameterSetRecommenderDetail } from "./ParameterSetRecommenderDetail";

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
      </Switch>
    </React.Fragment>
  );
};
