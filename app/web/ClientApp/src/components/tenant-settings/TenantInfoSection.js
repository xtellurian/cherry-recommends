import React from "react";
import { useCurrentTenant } from "../../api-hooks/tenantsApi";
import { Spinner } from "../molecules";
import { CopyableField } from "../molecules/fields/CopyableField";
import { CardSection, Label } from "../molecules/layout/CardSection";

export const TenantInfoSection = () => {
  const tenant = useCurrentTenant();

  if (tenant.loading) {
    return (
      <CardSection>
        <Spinner />
      </CardSection>
    );
  }
  return (
    <React.Fragment>
      <CardSection>
        <Label>Tenant Information</Label>
        <CopyableField label="Name" value={tenant.name} />
      </CardSection>
    </React.Fragment>
  );
};
