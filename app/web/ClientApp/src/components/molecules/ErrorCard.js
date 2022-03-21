import React from "react";
import { ExpandableCard } from "./ExpandableCard";
import { JsonView } from "./JsonView";

export const ErrorCard = ({ error }) => {
  if (!error) {
    return <React.Fragment />;
  }
  const validationErrorList = [];
  if (error.errors) {
    if (Array.isArray(error.errors)) {
      for (const e of error.errors) {
        validationErrorList.push(e);
      }
    } else {
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
  }
  let headerClassName = "bg-warning";
  switch (error.status) {
    case 404:
      headerClassName = "bg-info";
      break;
    default:
      headerClassName = "bg-warning";
  }
  let title = error.title ? error.title : "Error";
  if (validationErrorList.length > 0) {
    const extra =
      validationErrorList[0].label ?? validationErrorList[0].message;
    title = `${title} - ${extra}`;
  }

  return (
    <React.Fragment>
      <ExpandableCard label={title} headerClassName={headerClassName}>
        {validationErrorList.map((e) => (
          <div key={e.label ?? e.message}>
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
