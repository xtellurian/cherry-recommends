import React from "react";
import { ExpandableCard } from "./ExpandableCard";
import { JsonView } from "./JsonView";

export const ErrorCard = ({ error }) => {
  let headerClassName = "bg-warning";
  switch (error.status) {
    case 404:
      headerClassName = "bg-info";
      break;
    default:
      headerClassName = "bg-warning";
  }

  return (
    <React.Fragment>
      <ExpandableCard
        label={error.title || "Error"}
        headerClassName={headerClassName}
      >
        <JsonView data={error} />
      </ExpandableCard>
    </React.Fragment>
  );
};
