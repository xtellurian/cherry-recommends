import React from "react";

export const AsyncButton = ({
  className,
  onClick,
  children,
  loading,
  disabled,
  ...props
}) => {
  return (
    <button
      onClick={onClick}
      className={className || "btn btn-primary"}
      type="button"
      disabled={loading || disabled}
      {...props}
    >
      {loading && (
        <React.Fragment>
          <span
            className="spinner-border spinner-border-sm"
            role="status"
            aria-hidden="true"
          ></span>
          <span className="sr-only">Loading...</span>
        </React.Fragment>
      )}
      {!loading && <React.Fragment>{children}</React.Fragment>}
    </button>
  );
};
