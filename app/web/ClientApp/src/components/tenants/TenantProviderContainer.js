import React from "react";
import { useHosting } from "./HostingProvider";
import { Spinner } from "../molecules";
import { PathTenantProvider } from "./PathTenantProvider";
import { TenantChecker } from "./TenantChecker";

export const TenantProviderContainer = ({ children }) => {
  const hosting = useHosting();
  if (hosting.loading) {
    return <Spinner />;
  }
  if (hosting.multitenant) {
    return (
      <PathTenantProvider>
        <TenantChecker hosting={hosting}>{children}</TenantChecker>
      </PathTenantProvider>
    );
  } else {
    return <React.Fragment>{children}</React.Fragment>;
  }
};
