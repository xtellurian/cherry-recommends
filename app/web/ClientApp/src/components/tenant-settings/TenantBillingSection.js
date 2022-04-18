import React from "react";
import { useTenantAccount } from "../../api-hooks/tenantsApi";
import { Spinner } from "../molecules";
import { CopyableField } from "../molecules/fields/CopyableField";
import { CardSection, Label } from "../molecules/layout/CardSection";

export const TenantBillingSection = () => {
  const account = useTenantAccount();

  if (account.loading) {
    return (
      <CardSection>
        <Spinner />
      </CardSection>
    );
  }
  return (
    <React.Fragment>
      <CardSection>
        <Label>Billing Details</Label>
        <CopyableField
          label="Plan"
          inputClassName="text-capitalize"
          value={account.planType}
        />
        <a href="https://cherry.ai#price">
          <button className="btn btn-primary">Change Plan</button>
        </a>
      </CardSection>
    </React.Fragment>
  );
};
