import React from "react";
import { useCurrentTenant } from "../../api-hooks/tenantsApi";
import { Spinner } from "../molecules";
import { useTenantName } from "./PathTenantProvider";
import { TenantNotFound } from "./TenantNotFound";

export const TenantChecker = ({ children, hosting }) => {
  const tenant = useCurrentTenant();
  const { tenantName } = useTenantName();
  if (tenant.loading || hosting.loading) {
    return <Spinner />;
  }
  console.debug(
    `Tenant Name Checked: ${tenantName}. Comparing to ${tenant?.name}`
  );

  if (hosting.multitenant === false) {
    return <React.Fragment>{children}</React.Fragment>;
  } else if (!tenantName) {
    return <React.Fragment>{children}</React.Fragment>;
  } else if (tenant.error) {
    return <TenantNotFound error={tenant.error} />;
  } else {
    return <React.Fragment>{children}</React.Fragment>;
  }
};
