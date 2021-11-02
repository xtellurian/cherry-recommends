import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ParameterSetRecommendersComponent } from "./parameterset-recommenders/ParameterSetRecommendersComponent";
import { ItemsRecommendersComponent } from "./items-recommenders/ItemsRecommendersComponent";

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
          path={`${path}/items-recommenders`}
          component={ItemsRecommendersComponent}
        />
      </Switch>
    </React.Fragment>
  );
};
