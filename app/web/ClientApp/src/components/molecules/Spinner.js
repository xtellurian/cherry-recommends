import React from "react";

export const Spinner = ({ children, noText }) => {
  return (
    <div className="justify-content-center">
      <div className="d-flex justify-content-center mt-3 text-info">
        <div className="spinner-border" role="status"></div>
      </div>
      <div className="mt-3 text-center text-muted">
        {children || (noText ? null : "Loading...")}
      </div>
    </div>
  );
};
