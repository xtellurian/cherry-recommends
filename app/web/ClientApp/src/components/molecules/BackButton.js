import React from "react";
import { Link } from "react-router-dom";
import { ArrowLeft } from "react-bootstrap-icons";

import "./css/nav.css";

export const BackButton = ({ to, children, className }) => {
  return (
    <div className={className}>
      <Link to={to}>
        <button className="btn btn-outline-primary">
          <ArrowLeft size={18} className="mr-2" />
          {children}
        </button>
      </Link>
    </div>
  );
};

export const PrimaryBackButton = ({ to, children, className }) => {
  return (
    <div className={className}>
      <Link to={to} className="primary-nav-link text-decoration-none">
        <ArrowLeft size={18} className="mr-2" />
        {children}
      </Link>
    </div>
  );
};
