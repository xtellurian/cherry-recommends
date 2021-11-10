import React from "react";
import { Link } from "react-router-dom";
import { EntityRow } from "./EntityRow";
export const ClickableRow = ({ label, to, buttonText }) => {
  return (
    <EntityRow>
      <div className="col-md-9">{label}</div>
      <div className="col-md-3">
        <Link to={to}>
          <button className="btn btn-primary btn-block">
            {buttonText || "Go"}
          </button>
        </Link>
      </div>
    </EntityRow>
  );
};
