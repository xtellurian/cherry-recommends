import React from "react";
import {
  useCurrentTenant,
  useHosting,
  managementSubdomain,
} from "../../api-hooks/tenantsApi";
import { Spinner } from "../molecules";
import { TenantNotFound } from "./TenantNotFound";
import { ManagementPage } from "./managementPage";

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
    window.location = `https://${managementSubdomain}.${hosting.canonicalRootDomain}`;
    return <Spinner />;
  } else if (hosting.isManagementSubdomain) {
    return <ManagementPage />;
  } else if (hosting.isWwwPage) {
    return <ManagementPage />;
  } else if (tenant.error) {
    return <TenantNotFound error={tenant.error} />;
  } else {
    return <React.Fragment>{children}</React.Fragment>;
  }
};
