import React from "react";

import { AsyncButton } from "../AsyncButton";

import "./EditPageLayout.css";

export const EditButton = ({ label, onClick, className, loading }) => {
  return (
    <AsyncButton
      loading={loading}
      onClick={onClick}
      className={`btn btn-primary edit-button ${className}`}
    >
      {label || "Save"}
    </AsyncButton>
  );
};

const EditPageLayout = ({
  children,
  editButton,
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
      <div>{editButton}</div>
    </div>
  );
};

export default EditPageLayout;
