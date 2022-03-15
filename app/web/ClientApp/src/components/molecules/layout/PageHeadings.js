import React from "react";

import "./PageHeadings.css";

export const Title = ({ children }) => {
  return <h1 className="text-capitalize">{children}</h1>;
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
      {title ? <div className="title text-capitalize">{title}</div> : null}
      {subtitle ? (
        <div className="subtitle text-capitalize">{subtitle}</div>
      ) : null}
      {showHr ? <hr /> : null}
    </div>
  );
};
