import React from "react";

export const EmptyState = ({ children }) => {
  return (
    <div
      className="w-100 border border-light text-center p-3"
      style={{ borderStyle: "dotted" }}
    >
      {children}
    </div>
  );
};
