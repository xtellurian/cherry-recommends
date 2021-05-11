import React from "react";

export const DemoLayout = ({ children, info, fakeName }) => {
  return (
    <React.Fragment>
      <div className="row">
        <div className="col">
          <div className="card">
            <div className="card-header bg-info text-center text-white">
              {fakeName} Consumer View</div>
            <div className="card-body p-4 bg-light">{children}</div>
          </div>
        </div>
        {!!info && <div className="col-3 mt-5 text-center">{info}</div>}
      </div>
    </React.Fragment>
  );
};
