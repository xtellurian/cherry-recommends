import React from "react";
import { Row as r } from "./Row";

export const Row = r;

export const Col = ({ className, children, columnClass }) => {
  return (
    <div className={`${columnClass || "col"} ${className || ""}`}>
      {children}
    </div>
  );
};
