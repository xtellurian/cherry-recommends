import React from "react";
import { useCurrentTenant } from "../../api-hooks/tenantsApi";

export const TenantInformation = () => {
  const tenant = useCurrentTenant();
  return <div className="text-capitalize">Tenant: {tenant.name}</div>;
};
