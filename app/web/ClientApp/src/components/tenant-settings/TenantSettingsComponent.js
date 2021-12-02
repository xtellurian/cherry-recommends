import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { TenantSettingsSummary } from "./TenantSettingsSummary";

export const TenantSettingsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <Switch>
      <AuthorizeRoute
        exact
        path={`${path}`}
        component={TenantSettingsSummary}
      />
    </Switch>
  );
};
