import React from "react";
import EntityFlexRow from "../molecules/layout/EntityFlexRow";

const MembershipRow = ({ tenant }) => {
  const link = `https://${window.location.host}/${tenant.name}?autoSignIn=true`;
  return (
    <EntityFlexRow>
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
    </EntityFlexRow>
  );
};

export default MembershipRow;
