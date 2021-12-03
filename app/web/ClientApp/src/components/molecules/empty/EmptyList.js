import React from "react";
import { EmptyState } from "./EmptyState";
import { EmptyStateText } from "./EmptyStateText";
export const EmptyList = ({ children }) => {
  return (
    <EmptyState>
      <EmptyStateText className="text-muted p-4 m-4">{children}</EmptyStateText>
    </EmptyState>
  );
};
