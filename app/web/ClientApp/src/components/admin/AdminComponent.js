import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { TrackedUserFeaturesAdminComponent } from "./tracked-user-features/TrackedUserFeaturesAdminComponent";
import { FeatureGeneratorsComponent } from "./feature-generators/FeatureGeneratorsComponent";

export const AdminComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <div className="border border-warning rounded text-muted text-center mb-2">
        Admin Zone
      </div>
      <Switch>
        <AuthorizeRoute
          path={`${path}/features`}
          component={TrackedUserFeaturesAdminComponent}
        />
        <AuthorizeRoute
          path={`${path}/feature-generators`}
          component={FeatureGeneratorsComponent}
        />
      </Switch>
    </React.Fragment>
  );
};
