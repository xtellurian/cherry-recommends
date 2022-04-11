import React from "react";

export const InputLabel = ({ children, required }) => {
  return (
    <div>
      {children}{" "}
      <label className="text-muted">
        ({required ? "Required" : "Optional"})
      </label>
    </div>
  );
};
