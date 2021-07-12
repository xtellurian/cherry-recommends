import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { ProductRecommendersSummary } from "./ProductRecommendersSummary";
import { CreateProductRecommender } from "./CreateProductRecommender";
import { ProductRecommenderDetail } from "./ProductRecommenderDetail";
import { TestProductRecommender } from "./TestProductRecommender";
import { RecommendationList } from "./RecommendationList";
import { IntegrateProductRecommender } from "./IntegrateProductRecommender";
import { LinkToModel } from "./LinkToModel";

export const ProductRecommendersComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute
          exact
          path={`${path}`}
          component={ProductRecommendersSummary}
        />
        <AuthorizeRoute
          exact
          path={`${path}/create`}
          component={CreateProductRecommender}
        />
        <AuthorizeRoute
          exact
          path={`${path}/detail/:id`}
          component={ProductRecommenderDetail}
        />
        <AuthorizeRoute
          path={`${path}/link-to-model/:id`}
          component={LinkToModel}
        />
        <AuthorizeRoute
          path={`${path}/test/:id`}
          component={TestProductRecommender}
        />
        <AuthorizeRoute
          path={`${path}/recommendations/:id`}
          component={RecommendationList}
        />
        <AuthorizeRoute
          path={`${path}/integrate/:id`}
          component={IntegrateProductRecommender}
        />
      </Switch>
    </React.Fragment>
  );
};