import React from "react";
import { PersonCircle } from "react-bootstrap-icons";
import { useCurrentTenantMemberships } from "../../api-hooks/tenantsApi";
import { ErrorCard, Spinner } from "../molecules";
import { EntityRow } from "../molecules/layout/EntityRow";

const MembershipRow = ({ membership }) => {
  return (
    <EntityRow>
      <div className="col-1">
        <PersonCircle />
      </div>
      <div className="col">{membership.email}</div>
      <div className="col-3 text-right">
        {membership.emailVerified ? "Member" : "Pending"}
      </div>
    </EntityRow>
  );
};
export const TenantMembersSection = () => {
  const memberships = useCurrentTenantMemberships();
  return (
    <>
      <div>
        <h5>Team</h5>
        {memberships.loading && <Spinner />}
        {memberships.error && <ErrorCard error={memberships.error} />}
        {memberships.items &&
          memberships.items.map((m) => (
            <MembershipRow key={m.userId} membership={m} />
          ))}
      </div>
    </>
  );
};
