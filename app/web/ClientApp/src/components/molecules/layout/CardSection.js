import React from "react";
import { Navigation } from "../Navigation";

export const CardSection = ({ children, className }) => {
  return (
    <div className={`mt-1 mb-1 card-section ${className || ""}`}>
      {children}
    </div>
  );
};

export const Label = ({ children }) => {
  return <h6 className="font-weight-bold text-capitalize mb-2">{children}</h6>;
};

export const MoreLink = ({ to, children }) => {
  return (
    <div className="text-center text-muted mt-1">
      <Navigation to={to}>
        <button className="btn btn-link">{children}</button>
      </Navigation>
    </div>
  );
};
