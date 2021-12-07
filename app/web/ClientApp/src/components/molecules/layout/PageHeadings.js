import React from "react";

export const Title = ({ children }) => {
  return <h1 className="text-capitalize">{children}</h1>;
};

export const PageSectionTitle = ({ children }) => {
  return <h2 className="text-capitalize">{children}</h2>;
};

export const Subtitle = ({ children }) => {
  return <h5 className="text-capitalize">{children}</h5>;
};
