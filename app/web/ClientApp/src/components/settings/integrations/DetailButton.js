import React from "react";
import { Link } from "react-router-dom";

export const DetailButton = ({ integratedSystem, className }) => {
  if (integratedSystem.systemType === "hubspot") {
    return (
      <Link to={`/settings/integrations/hubspot-detail/${integratedSystem.id}`}>
        <button className={`btn btn-primary ${className || ""}`}>
          More Options
        </button>
      </Link>
    );
  } else if (integratedSystem.systemType === "custom") {
    return (
      <Link to={`/settings/integrations/custom/${integratedSystem.id}`}>
        <button className={`btn btn-primary ${className || ""}`}>
          More Options
        </button>
      </Link>
    );
  } else {
    return <React.Fragment />;
  }
};
