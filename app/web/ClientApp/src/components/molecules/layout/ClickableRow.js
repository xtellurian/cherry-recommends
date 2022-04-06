import React from "react";
import { Navigation } from "../Navigation";
import { EntityRow } from "./EntityRow";
export const ClickableRow = ({ label, to, buttonText }) => {
  return (
    <EntityRow>
      <div className="col-md-9">{label}</div>
      <div className="col-md-3">
        <Navigation to={to}>
          <button className="btn btn-outline-primary btn-block">
            {buttonText || "Go"}
          </button>
        </Navigation>
      </div>
    </EntityRow>
  );
};
