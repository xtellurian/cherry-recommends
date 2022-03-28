import React from "react";
import { Link } from "react-router-dom";

export const DetailButton = ({ integratedSystem, className }) => {
  let link = "";
  switch (integratedSystem.systemType) {
    case "hubspot":
      link = `/settings/integrations/hubspot-detail/${integratedSystem.id}`;
      break;
    case "custom":
      link = `/settings/integrations/custom/${integratedSystem.id}`;
      break;
    default:
      break;
  }
  if (link) {
    return (
      <Link to={link}>
        <button className={`btn btn-primary ${className || ""}`}>
          More Options
        </button>
      </Link>
    );
  } else {
    return <React.Fragment />;
  }
};
