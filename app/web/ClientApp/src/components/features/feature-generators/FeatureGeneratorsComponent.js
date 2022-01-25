import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { FeatureGeneratorsSummary } from "./FeatureGeneratorsSummary";

export const FeatureGeneratorsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute
          exact
          path={`${path}`}
          component={FeatureGeneratorsSummary}
        />
      </Switch>
    </React.Fragment>
  );
};
