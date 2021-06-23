import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ParameterSetRecommendersComponent } from "./parametersetrecommenders/ParameterSetRecommendersComponent";

export const RecommendersComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute
          path={`${path}/parameter-set-recommenders`}
          component={ParameterSetRecommendersComponent}
        />
      </Switch>
    </React.Fragment>
  );
};
