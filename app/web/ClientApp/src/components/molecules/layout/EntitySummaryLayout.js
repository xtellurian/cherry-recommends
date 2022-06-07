import React from "react";

import { ErrorCard } from "../ErrorCard";
import { Navigation } from "../Navigation";
import { PageHeading, Title } from "./PageHeadings";

export const CreateEntityButton = ({ to, children }) => {
  return (
    <Navigation to={to}>
      <button className="btn btn-primary">
        {children ? children : "Create"}
      </button>
    </Navigation>
  );
};

const EntitySummaryPageLayout = ({
  children,
  createButton,
  header,
  headerDivider = true,
  error,
}) => {
  return (
    <div className="clearfix">
      <div className="float-right">{createButton}</div>
      {typeof header === "string" ? <PageHeading title={header} /> : header}
      {headerDivider ? <hr className="mb-4" /> : null}
      {error ? (
        <div className="mb-4">
          <ErrorCard error={error} />
        </div>
      ) : null}
      <div>{children}</div>
    </div>
  );
};

export default EntitySummaryPageLayout;
