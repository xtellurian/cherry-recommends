import React from "react";

import { Typography } from "../../molecules";

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

export const PageHeading = ({ title, subtitle }) => {
  return (
    <div>
      <Typography variant="h5" className="semi-bold mb-0">
        {title}
      </Typography>
      <Typography className="text-secondary">{subtitle}</Typography>
    </div>
  );
};
