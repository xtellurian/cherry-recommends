import React from "react";
import {
  useCurrentTenant,
  useHosting,
  managementSubdomain,
} from "../../api-hooks/tenantsApi";
import { Spinner } from "../molecules";
import { TenantNotFound } from "./TenantNotFound";
import { TenantsComponent } from "./TenantsComponent";

export const TenantChecker = ({ children }) => {
  const tenant = useCurrentTenant();
  const hosting = useHosting();
  console.log(hosting);
  if (tenant.loading || hosting.loading) {
    return <Spinner />;
  }
  if (hosting.multitenant === false) {
    return <React.Fragment>{children}</React.Fragment>;
  } else if (hosting.isCanonicalRoot) {
    window.location = `https://${managementSubdomain}.${hosting.canonicalRootDomain}?autoSignIn=true`;
    return <Spinner />;
  } else if (hosting.isManagementSubdomain) {
    return <TenantsComponent />;
  } else if (hosting.isWwwPage) {
    return <TenantsComponent />;
  } else if (tenant.error) {
    return <TenantNotFound error={tenant.error} />;
  } else {
    return <React.Fragment>{children}</React.Fragment>;
  }
};
