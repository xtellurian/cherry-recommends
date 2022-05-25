import React from "react";

import { AsyncButton } from "../AsyncButton";
import { ErrorCard } from "../ErrorCard";

import "./CreatePageLayout.css";

export const CreateButton = ({
  label,
  onCreate,
  className,
  loading,
  disabled,
}) => {
  return (
    <AsyncButton
      loading={loading}
      onClick={onCreate}
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
