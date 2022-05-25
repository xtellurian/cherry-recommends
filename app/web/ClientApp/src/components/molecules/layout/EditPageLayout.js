import React from "react";

import { AsyncButton } from "../AsyncButton";
import { ErrorCard } from "../ErrorCard";

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
      <div className="mt-4">{editButton}</div>
    </div>
  );
};

export default EditPageLayout;
