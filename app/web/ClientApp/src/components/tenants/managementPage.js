import React from "react";
import { useHosting, useMemberships } from "../../api-hooks/tenantsApi";
import { Spinner } from "../molecules";

const MembershipRow = ({ tenant, hosting }) => {
  const link = `https://${tenant.name}.${hosting.canonicalRootDomain}`;
  return (
    <div className="row mt-2 text-center justify-content-center">
      <div className="col-6">
        <a href={link}>
          <button className="btn btn-outline-primary btn-block text-capitalize">
            {tenant.name}
          </button>
        </a>
      </div>
    </div>
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
    <React.Fragment>
      <div className="m-2 text-center">
        <div className="text-center">
          <h1 className="display-3">Four 2</h1>
        </div>

        <h4>You are a part of the following organisations</h4>


        {memberships.loading && <Spinner>Loading User Information</Spinner>}

        <div className="m-3">
          {memberships.length > 0 &&
            memberships.map((m) => (
              <MembershipRow key={m.name} tenant={m} hosting={hosting} />
            ))}
        </div>
      </div>
    </React.Fragment>
  );
};
