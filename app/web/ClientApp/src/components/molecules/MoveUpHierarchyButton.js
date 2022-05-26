import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAnglesUp as icon } from "@fortawesome/free-solid-svg-icons";

import { Typography } from "../molecules";
import { Navigation } from "./Navigation";

import "./css/nav.css";

export const MoveUpHierarchyButton = ({ to, children, className }) => {
  return (
    <div className={className}>
      <Navigation to={to}>
        <button className="btn btn-outline-primary d-flex align-items-center">
          <FontAwesomeIcon icon={icon} fontSize={16} className="mr-2" />

          {children}
        </button>
      </Navigation>
    </div>
  );
};

export const MoveUpHierarchyPrimaryButton = ({ to, children, className }) => {
  return (
    <div className={`mb-4 ${className}`}>
      <Navigation to={to} className="primary-nav-link text-decoration-none">
        <FontAwesomeIcon icon={icon} fontSize={16} className="mr-2" />
        <Typography
          component="span"
          className="semi-bold"
          style={{ color: "var(--cherry-purple)" }}
        >
          {children}
        </Typography>
      </Navigation>
    </div>
  );
};
