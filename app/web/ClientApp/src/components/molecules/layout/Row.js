import React from "react";

export const Row = ({ children, className }) => {
  return <div className={`row ${className || ""}`}>{children}</div>;
};
