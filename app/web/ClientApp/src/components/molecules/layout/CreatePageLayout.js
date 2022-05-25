import React from "react";

import { AsyncButton } from "../AsyncButton";
import { ErrorCard } from "../ErrorCard";

import "./CreatePageLayout.css";

export const CreateButton = ({
  label,
  onClick,
  className,
  loading,
  disabled,
}) => {
  return (
    <AsyncButton
      loading={loading}
      onClick={onClick}
      className={`btn btn-primary create-button ${className}`}
      disabled={disabled}
    >
      {label || "Create"}
    </AsyncButton>
  );
};

const CreatePageLayout = ({
  children,
  createButton,
  backButton,
  header,
  headerDivider = true,
  error,
}) => {
  return (
    <div className="clearfix">
      {backButton}
      {header}
      {headerDivider ? <hr className="mb-4" /> : null}
      {error ? (
        <div className="mb-4">
          <ErrorCard error={error} />
        </div>
      ) : null}
      {children}
      <div className="mt-4">{createButton}</div>
    </div>
  );
};

export default CreatePageLayout;
