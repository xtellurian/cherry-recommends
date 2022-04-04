import React from "react";

import "./PageHeadings.css";

export const Title = ({ children, ...props }) => {
  return (
    <h1 className="text-capitalize" {...props}>
      {children}
    </h1>
  );
};

export const PageSectionTitle = ({ children }) => {
  return <h2 className="text-capitalize">{children}</h2>;
};

export const Subtitle = ({ children }) => {
  return <h5 className="text-capitalize">{children}</h5>;
};

export const PageHeading = ({ title, subtitle, showHr }) => {
  return (
    <div className="page-heading-wrapper mb-4">
      <div className="title text-capitalize">{title}</div>
      <div className="subtitle text-capitalize">{subtitle}</div>
      {showHr ? <hr /> : null}
    </div>
  );
};
