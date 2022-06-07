import React from "react";
import { Typography } from "../Typography";

export const SectionHeading = ({ children }) => {
  return (
    <Typography variant="h4" className="text-capitalize mb-2">
      {children}
    </Typography>
  );
};
