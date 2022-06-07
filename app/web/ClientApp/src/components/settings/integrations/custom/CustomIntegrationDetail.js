import React from "react";
import { MoveUpHierarchyPrimaryButton, PageHeading } from "../../../molecules";
import { CopyableField } from "../../../molecules/fields/CopyableField";

export const CustomIntegrationDetail = ({ integratedSystem }) => {
  return (
    <React.Fragment>
      <MoveUpHierarchyPrimaryButton
        to={`/settings/integrations/detail/${integratedSystem.id}`}
      >
        Back
      </MoveUpHierarchyPrimaryButton>
      <PageHeading
        title={integratedSystem.name}
        subtitle="Custom Integration"
      />

      <div className="pt-3">
        <CopyableField
          label="Application Id"
          value={integratedSystem.applicationId}
        />
        <CopyableField
          label="Application Secret"
          value={integratedSystem.applicationSecret}
          isSecret={true}
        />
      </div>
    </React.Fragment>
  );
};
