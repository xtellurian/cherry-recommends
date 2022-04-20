import React from "react";

export const LinkToDocs = ({ children }) => {
  return (
    <a target="_blank" href="https://docs.cherry.ai/docs/integrations/library">
      {children}
    </a>
  );
};
