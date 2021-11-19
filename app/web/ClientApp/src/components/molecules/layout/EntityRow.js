import React from "react";

export const EntityRow = ({ children }) => {
  return (
    <div className="row mt-1 mb-2 p-3 shadow-sm bg-body rounded">
      {children}
    </div>
  );
};
