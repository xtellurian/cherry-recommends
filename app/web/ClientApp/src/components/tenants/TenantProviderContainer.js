import React from "react";
import { Outlet } from "react-router-dom";

import { useHosting } from "./HostingProvider";
import { Spinner } from "../molecules";
import { PathTenantProvider } from "./PathTenantProvider";
import { TenantChecker } from "./TenantChecker";
import { MembershipsProvider } from "./MembershipsProvider";

export const TenantProviderContainer = () => {
  const hosting = useHosting();
  if (hosting.loading) {
    return <Spinner />;
  }

  if (hosting.multitenant) {
    return (
      <MembershipsProvider>
        <PathTenantProvider>
          <TenantChecker hosting={hosting}>
            <Outlet />
          </TenantChecker>
        </PathTenantProvider>
      </MembershipsProvider>
    );
  }

  return <Outlet />;
};
