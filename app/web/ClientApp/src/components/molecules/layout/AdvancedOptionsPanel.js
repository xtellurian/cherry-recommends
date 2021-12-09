import React from "react";
import { ExpandableCard } from "..";

export const AdvancedOptionsPanel = ({ children }) => {
  return (
    <>
      <ExpandableCard label="Advanced Options">{children}</ExpandableCard>
    </>
  );
};
