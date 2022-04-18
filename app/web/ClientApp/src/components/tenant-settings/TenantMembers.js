import React from "react";
import { PersonCircle } from "react-bootstrap-icons";
import { useCurrentTenantMemberships } from "../../api-hooks/tenantsApi";
import { ErrorCard, Spinner } from "../molecules";
import { CardSection, Label } from "../molecules/layout/CardSection";
import EntityRow from "../molecules/layout/EntityFlexRow";

const MembershipRow = ({ membership }) => {
  return (
    <EntityRow>
      <div>
        <PersonCircle />
        <span className="text-bold ml-5">{membership.email}</span>
      </div>
      <div>{membership.emailVerified ? "Member" : "Pending"}</div>
    </EntityRow>
  );
};
export const TenantMembersSection = ({ children }) => {
  const memberships = useCurrentTenantMemberships();
  return (
    <>
      <CardSection>
        <div>
          <Label>Team</Label>
          {memberships.loading && <Spinner />}
          {memberships.error && <ErrorCard error={memberships.error} />}
          {memberships.items &&
            memberships.items.map((m) => (
              <MembershipRow key={m.userId} membership={m} />
            ))}
        </div>
        {children}
      </CardSection>
    </>
  );
};
