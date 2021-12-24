import React from "react";

export const CardSection = ({ children }) => {
  return <div className="mt-1 mb-1 card-section">{children}</div>;
};

export const Label = ({ children }) => {
  return <h6 className="font-weight-bold text-capitalize mb-2">{children}</h6>;
};
