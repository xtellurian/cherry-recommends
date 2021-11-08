import React from "react";

export const SettingRow = ({ label, description, children }) => {
  return (
    <React.Fragment>
      <div className="row mt-1 mb-2 p-3 shadow-sm bg-body rounded">
        <div className="col">
          <h5>{label}</h5>
          {description}
        </div>
        <div className="col-lg-6 col-md-8 text-center">{children}</div>
      </div>
    </React.Fragment>
  );
};
