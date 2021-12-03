import React from "react";
import { ExpandableCard } from "./ExpandableCard";
import { JsonView } from "./JsonView";

export const ErrorCard = ({ error }) => {
  if (!error) {
    return <React.Fragment />;
  }
  const validationErrorList = [];
  if (error.errors) {
    for (const [label, errorMessage] of Object.entries(error.errors)) {
      if (typeof errorMessage === "string") {
        validationErrorList.push({
          label,
          message: errorMessage,
        });
      } else if (
        errorMessage.message &&
        typeof errorMessage.message === "string"
      ) {
        validationErrorList.push({
          label,
          message: errorMessage.message,
        });
      }
    }
  }
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
        {validationErrorList.map((e) => (
          <div key={e.label}>
            <strong>{e.label}:</strong>
            {e.message}
          </div>
        ))}
        <ExpandableCard label="More Detail">
          <JsonView data={error} />
        </ExpandableCard>
      </ExpandableCard>
    </React.Fragment>
  );
};
