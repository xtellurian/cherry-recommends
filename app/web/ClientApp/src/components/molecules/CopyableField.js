import React from "react";

export const CopyableField = ({ label, value, isSecret }) => {
  return (
    <div className="input-group mb-3">
      <div className="input-group-prepend ml-1">
        <span className="input-group-text">{label}</span>
      </div>
      <input
        type={isSecret ? "password" : "text"}
        value={value || ""} // dont allow uncontrolled
        className="form-control"
        aria-label={label}
        disabled
      />
      <button
        className="btn btn-outline-secondary"
        type="button"
        onClick={() => navigator.clipboard.writeText(value)}
      >
        Copy
      </button>
    </div>
  );
};
