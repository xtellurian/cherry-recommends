import React from "react";

export const DateTimeField = ({ label, date }) => {
  let dateValue = new Date();
  if (typeof date === "string") {
    dateValue = new Date(Date.parse(date));
  } else if (typeof date === "number") {
    dateValue = new Date(date);
  } else {
    return <div>{date} is not a known date format</div>;
  }

  return (
    <div>
      {label}:{" "}{dateValue.toLocaleString()}
    </div>
  );
};
