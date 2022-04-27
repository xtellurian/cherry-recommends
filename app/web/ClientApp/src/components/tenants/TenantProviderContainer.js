import React from "react";
import { useHosting } from "./HostingProvider";
import { Spinner } from "../molecules";
import { PathTenantProvider } from "./PathTenantProvider";
import { TenantChecker } from "./TenantChecker";
import { MembershipsProvider } from "./MembershipsProvider";

export const TenantProviderContainer = ({ children }) => {
  const hosting = useHosting();
  if (hosting.loading) {
    return <Spinner />;
  }
  if (hosting.multitenant) {
    return (
      <MembershipsProvider>
        <PathTenantProvider>
          <TenantChecker hosting={hosting}>{children}</TenantChecker>
        </PathTenantProvider>
      </MembershipsProvider>
    );
  } else {
    return <React.Fragment>{children}</React.Fragment>;
  }
};
