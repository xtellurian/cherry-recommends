import React from "react";

const CreatePageLayout = ({ children, createButton }) => {
  return (
    <React.Fragment>
      <span className="float-right">{createButton}</span>
      {children}
    </React.Fragment>
  );
};

export default CreatePageLayout;
