import React from "react";
import { Redirect, useRouteMatch } from "react-router-dom";
import { useMemberships } from "../../api-hooks/tenantsApi";
import { Spinner, Title, Subtitle, Navigation } from "../molecules";
import { EntityRow } from "../molecules/layout/EntityRow";

const MembershipRow = ({ tenant }) => {
  const link = `https://${window.location.host}/${tenant.name}?autoSignIn=true`;
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
  const { path } = useRouteMatch();
  if (!memberships.loading && memberships.length === 0) {
    return (
      <React.Fragment>
        <Redirect to={`${path}/create-tenant`} />
      </React.Fragment>
    );
  }

  return (
    <div className="container">
      <div className="m-2">
        <div className="text-center mt-5 mb-5">
          <Title className="display-5">🍒 Cherry 🍒</Title>
        </div>

        <div className="mb-5">
          <Navigation to={`${path}/create-tenant`}>
            <button className="btn btn-primary float-right">
              Create a new Tenant
            </button>
          </Navigation>
          <Subtitle>Tenant Management</Subtitle>
        </div>
        <hr />

        <h3>You are a part of the following tenants</h3>
        {memberships.loading && <Spinner>Loading User Information</Spinner>}

        {memberships.error && (
          <div className="text-center pt-5">
            <h4>Error</h4>
            <p>{memberships.error.title}</p>
            <a href="mailto:rian@cherry.ai">
              <button className="btn btn-outline-danger">Report Issue</button>
            </a>
          </div>
        )}
        <div className="m-3">
          {memberships.length > 0 &&
            memberships.map((m) => <MembershipRow key={m.name} tenant={m} />)}
        </div>
      </div>
    </div>
  );
};
