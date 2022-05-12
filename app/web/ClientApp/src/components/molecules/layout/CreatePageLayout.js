import React from "react";

const CreatePageLayout = ({
  children,
  createButton,
  backButton,
  header,
  headerDivider = true,
}) => {
  return (
    <React.Fragment>
      {backButton}
      <div className="row">
        <div className="col d-flex align-items-center">{header}</div>
        {createButton ? (
          <div className="col d-flex justify-content-end align-items-center">
            {createButton}
          </div>
        ) : null}
      </div>
      {headerDivider ? <hr className="mb-4" /> : null}
      {children}
    </React.Fragment>
  );
};

export default CreatePageLayout;
