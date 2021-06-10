import React from "react";
import { ExpandableCard } from "./ExpandableCard";
import { JsonView } from "./JsonView";

export const ErrorCard = ({ error }) => {
  return (
    <React.Fragment>
      <ExpandableCard name={error.title || "Error"} headerClassName="bg-warning">
        <JsonView data={error} />
      </ExpandableCard>
    </React.Fragment>
  );
};
