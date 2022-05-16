import React from "react";

const EntityDetailPageLayout = ({
  children,
  backButton,
  header,
  options,
  headerDivider = true,
}) => {
  return (
    <React.Fragment>
      {backButton}
      <div className="row">
        <div className="col">{header}</div>
        {options ? (
          <div className="col d-flex justify-content-end align-items-start">
            {options}
          </div>
        ) : null}
      </div>
      {headerDivider ? <hr className="mb-4" /> : null}
      {children}
    </React.Fragment>
  );
};

export default EntityDetailPageLayout;
