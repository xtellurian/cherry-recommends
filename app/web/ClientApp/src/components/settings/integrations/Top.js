import React from "react";

import { MoveUpHierarchyPrimaryButton, PageHeading } from "../../molecules";
import { DetailButton } from "./DetailButton";

export const Top = ({ integratedSystem }) => {
  if (!integratedSystem || integratedSystem.loading) {
    return (
      <React.Fragment>
        <MoveUpHierarchyPrimaryButton to="/settings/integrations">
          Back to Integrations
        </MoveUpHierarchyPrimaryButton>
        <PageHeading title="Integrated System" subtitle="..." />
      </React.Fragment>
    );
  }

  return (
    <React.Fragment>
      <MoveUpHierarchyPrimaryButton to="/settings/integrations">
        Back to Integrations
      </MoveUpHierarchyPrimaryButton>
      <DetailButton
        className="float-right mr-1"
        integratedSystem={integratedSystem}
      />
      <PageHeading
        title={integratedSystem.name || integratedSystem.name || "..."}
        subtitle="Integrated System"
      />
    </React.Fragment>
  );
};
