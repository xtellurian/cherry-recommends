import React from "react";
import { EntityRow } from "./EntityRow";

export const SettingRow = ({ label, description, children }) => {
  return (
    <React.Fragment>
      <EntityRow>
        <div className="col">
          <h5>{label}</h5>
          {description}
        </div>
        <div className="col-lg-6 col-md-8 text-center">{children}</div>
      </EntityRow>
    </React.Fragment>
  );
};
