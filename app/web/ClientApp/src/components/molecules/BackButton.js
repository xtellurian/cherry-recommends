import React from "react";
import { ArrowLeft } from "react-bootstrap-icons";

import { Typography } from "../molecules";
import { Navigation } from "./Navigation";

import "./css/nav.css";

// Warning: This component was changed to MoveUpHierarchyButton, use it instead.
export const BackButton = ({ to, children, className }) => {
  return (
    <div className={className}>
      <Navigation to={to}>
        <button className="btn btn-outline-primary d-flex align-items-center">
          <ArrowLeft size={18} className="mr-2" />
          {children}
        </button>
      </Navigation>
    </div>
  );
};

// Warning: This component was changed to MoveUpHierarchyPrimaryButton, use it instead.
export const PrimaryBackButton = ({ to, children, className = "" }) => {
  return (
    <div className={`mb-3 ${className}`}>
      <Navigation
        to={to}
        className="d-flex align-items-center primary-nav-link text-decoration-none"
      >
        <ArrowLeft size={18} className="mr-2" />
        <Typography variant="h4">{children}</Typography>
      </Navigation>
    </div>
  );
};
