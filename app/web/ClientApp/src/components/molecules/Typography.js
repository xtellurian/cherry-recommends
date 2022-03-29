import React from "react";

import "./Typography.css";

export const Typography = ({
  children,
  variant = "h5",
  component = "span",
  className = "",
  gutterBottom,
  ...props
}) => {
  const _className = `typography ${className} ${variant} ${
    gutterBottom ? "gutterBottom" : ""
  }`;

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
