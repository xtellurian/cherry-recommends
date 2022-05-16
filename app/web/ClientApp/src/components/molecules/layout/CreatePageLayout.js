import React from "react";

import { AsyncButton } from "../AsyncButton";

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
}) => {
  return (
    <div className="clearfix">
      {backButton}
      {header}
      {headerDivider ? <hr className="mb-4" /> : null}
      {children}
      <div>{createButton}</div>
    </div>
  );
};

export default CreatePageLayout;
