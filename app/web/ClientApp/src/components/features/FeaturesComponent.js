import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { FeaturesSummary } from "./FeaturesSummary";
import { CreateFeature } from "./CreateFeature";
import { FeatureDetail } from "./FeatureDetail";
import { SetFeatureValue } from "./SetFeatureValue";
import { FeatureGeneratorsComponent } from "./feature-generators/FeatureGeneratorsComponent";

export const FeaturesComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={FeaturesSummary} />
        <AuthorizeRoute
          exact
          path={`${path}/create`}
          component={CreateFeature}
        />
        <AuthorizeRoute
          exact
          path={`${path}/detail/:id`}
          component={FeatureDetail}
        />
        <AuthorizeRoute
          exact
          path={`${path}/set-value/:id`}
          component={SetFeatureValue}
        />
        <AuthorizeRoute
          exact
          path={`${path}/generators`}
          component={FeatureGeneratorsComponent}
        />
      </Switch>
    </React.Fragment>
  );
};
