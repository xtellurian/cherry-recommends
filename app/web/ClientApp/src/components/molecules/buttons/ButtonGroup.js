import React from "react";

export const ButtonGroup = ({ className, ariaLabel, children }) => {
  return (
    <div
      className={`${className || ""} btn-group`}
      role="group"
      aria-label={ariaLabel || "Button Group"}
    >
      {children}
    </div>
  );
};
