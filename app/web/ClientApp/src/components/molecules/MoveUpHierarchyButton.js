import React from "react";
import { ArrowLeft } from "react-bootstrap-icons";

import { Typography } from "../molecules";
import { Navigation } from "./Navigation";

import "./css/nav.css";

export const MoveUpHierarchyButton = ({ to, children, className }) => {
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

export const MoveUpHierarchyPrimaryButton = ({
  to,
  children,
  className = "",
}) => {
  return (
    <div className={`mb-3 ${className}`}>
      <Navigation to={to} className="primary-nav-link text-decoration-none">
        <ArrowLeft size={18} className="mr-2" />
        <Typography variant="h4">{children}</Typography>
      </Navigation>
    </div>
  );
};
