import React from "react";
import { toDate } from "../../utility/utility";

export const DateTimeField = ({ label, date }) => {
  const dateValue = toDate(date);

  if (!dateValue) {
    return <div>{date} is not a known date format</div>;
  }

  return (
    <div>
      {label ? `${label}:` : null} {dateValue.toLocaleString()}
    </div>
  );
};
