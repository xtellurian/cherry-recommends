import React from "react";

import "./Typography.css";

export const Typography = ({
  children,
  variant,
  component = "div",
  className = "",
  ...props
}) => {
  const _className = `typography ${className} ${variant}`;

  if (component === "div") {
    return (
      <div className={_className} {...props}>
        {children}
      </div>
    );
  }

  return (
    <span className={_className} {...props}>
      {children}
    </span>
  );
};
