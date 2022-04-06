import React from "react";
import { Navigation } from "../../molecules";

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
      <Navigation to={link}>
        <button className={`btn btn-primary ${className || ""}`}>
          More Options
        </button>
      </Navigation>
    );
  } else {
    return <React.Fragment />;
  }
};
