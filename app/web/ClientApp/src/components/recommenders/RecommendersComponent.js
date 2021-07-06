import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ParameterSetRecommendersComponent } from "./parameterset-recommenders/ParameterSetRecommendersComponent";
import { ProductRecommendersComponent } from "./product-recommenders/ProductRecommendersComponent";

export const RecommendersComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute
          path={`${path}/parameter-set-recommenders`}
          component={ParameterSetRecommendersComponent}
        />
        <AuthorizeRoute
          path={`${path}/product-recommenders`}
          component={ProductRecommendersComponent}
        />
      </Switch>
    </React.Fragment>
  );
};
