import React from "react";
import { Title, Subtitle, MoveUpHierarchyButton } from "../../../molecules";
import { CopyableField } from "../../../molecules/fields/CopyableField";

export const CustomIntegrationSummary = ({ integratedSystem }) => {
  return (
    <React.Fragment>
      <MoveUpHierarchyButton
        className="float-right"
        to={`/settings/integrations/detail/${integratedSystem.id}`}
      >
        Back
      </MoveUpHierarchyButton>
      <Title>Custom Integration</Title>
      <Subtitle>{integratedSystem.name}</Subtitle>
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
