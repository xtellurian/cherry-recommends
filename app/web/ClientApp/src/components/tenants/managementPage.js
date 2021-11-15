import React from "react";
import { Link } from "react-router-dom";
import { useHosting, useMemberships } from "../../api-hooks/tenantsApi";
import { Spinner, Title, Subtitle } from "../molecules";
import { EntityRow } from "../molecules/layout/EntityRow";

const MembershipRow = ({ tenant, hosting }) => {
  const link = `https://${tenant.name}.${hosting.canonicalRootDomain}?autoSignIn=true`;
  return (
    <EntityRow>
      <div className="col-6">
        <h5>{tenant.name}</h5>
      </div>
      <div className="col-6">
        <a href={link}>
          <button className="btn btn-outline-primary btn-block text-capitalize">
            Go
          </button>
        </a>
      </div>
    </EntityRow>
  );
};
export const ManagementPage = () => {
  const memberships = useMemberships();
  const hosting = useHosting();
  if (memberships.error) {
    return (
      <div className="text-center pt-5">
        <h3>Error</h3>
        <p>{memberships.error.title}</p>
        <a href="mailto:rian@four2.ai">
          <button className="btn btn-outline-danger">Report Issue</button>
        </a>
      </div>
    );
  }

  return (
    <div className="container">
      <div className="m-2">
        <div className="text-center">
          <Title className="display-3">Four 2</Title>
        </div>

        <div className="mb-5">
          <Link to="create-tenant">
            <button className="btn btn-primary float-right">
              Create a new Tenant
            </button>
          </Link>
          <Subtitle>Tenant Management</Subtitle>
        </div>
        <hr />

        <h3>You are a part of the following tenants</h3>
        {memberships.loading && <Spinner>Loading User Information</Spinner>}

        <div className="m-3">
          {memberships.length > 0 &&
            memberships.map((m) => (
              <MembershipRow key={m.name} tenant={m} hosting={hosting} />
            ))}
        </div>
      </div>
    </div>
  );
};
