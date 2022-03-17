import React from "react";
import { ArrowLeft } from "react-bootstrap-icons";

import { Navigation } from "./Navigation";

import "./css/nav.css";

export const BackButton = ({ to, children, className }) => {
  return (
    <div className={className}>
      <Navigation to={to}>
        <button className="btn btn-outline-primary">
          <ArrowLeft size={18} className="mr-2" />
          {children}
        </button>
      </Navigation>
    </div>
  );
};

export const PrimaryBackButton = ({ to, children, className }) => {
  return (
    <div className={className}>
      <Navigation to={to} className="primary-nav-link text-decoration-none">
        <ArrowLeft size={18} className="mr-2" />
        {children}
      </Navigation>
    </div>
  );
};
