import React from "react";
import { Title, Subtitle, BackButton } from "../../../molecules";
import { CopyableField } from "../../../molecules/fields/CopyableField";

export const CustomIntegrationSummary = ({ integratedSystem }) => {
  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to={`/settings/integrations/detail/${integratedSystem.id}`}
      >
        Back
      </BackButton>
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
