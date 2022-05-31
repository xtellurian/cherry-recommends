import React from "react";
import { Routes, Route } from "react-router-dom";

import { EmptyState } from "../molecules";
import { useHosting } from "../tenants/HostingProvider";
import { TenantSettingsSummary } from "./TenantSettingsSummary";

export const TenantSettingsComponent = () => {
  const hosting = useHosting();
  if (hosting.multitenant) {
    return (
      <Routes>
        <Route index element={<TenantSettingsSummary />} />
      </Routes>
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
