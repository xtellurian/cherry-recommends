import React from "react";

export const SettingRow = ({ label, description, children }) => {
  return (
    <React.Fragment>
      <div className="row">
        <div className="col-7">
          <h5>{label}</h5>
          {description}
        </div>
        <div className="col text-center">{children}</div>
      </div>
      <hr />
    </React.Fragment>
  );
};
