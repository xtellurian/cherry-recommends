import React from "react";

export const SettingRow = ({ label, description, children }) => {
  return (
    <React.Fragment>
      <div className="row mt-1 mb-1">
        <div className="col">
          <h5>{label}</h5>
          {description}
        </div>
        <div className="col-3 text-center">{children}</div>
      </div>
    </React.Fragment>
  );
};
