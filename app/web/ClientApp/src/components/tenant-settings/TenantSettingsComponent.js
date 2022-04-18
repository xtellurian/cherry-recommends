import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { EmptyState } from "../molecules";
import { useHosting } from "../tenants/HostingProvider";
import { TenantSettingsSummary } from "./TenantSettingsSummary";

export const TenantSettingsComponent = () => {
  const { path } = useRouteMatch();
  const hosting = useHosting();
  if (hosting.multitenant) {
    return (
      <Switch>
        <AuthorizeRoute
          exact
          path={`${path}`}
          component={TenantSettingsSummary}
        />
      </Switch>
    );
  } else if (hosting.loading) {
    return <React.Fragment />;
  } else {
    // single tenant
    return (
      <React.Fragment>
        <EmptyState>
          Tenant Settings are unavailable in this deployment.
        </EmptyState>
      </React.Fragment>
    );
  }
};
