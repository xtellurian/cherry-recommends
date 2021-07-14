import React from "react";
import { Link } from "react-router-dom";

export const ClickableRow = ({ label, to, buttonText }) => {
  return (
    <div className="row mb-1">
      <div className="col-md-9">{label}</div>
      <div className="col-md-3">
        <Link to={to}>
          <button className="btn btn-primary btn-block">{buttonText || "Go"}</button>
        </Link>
      </div>
    </div>
  );
};
