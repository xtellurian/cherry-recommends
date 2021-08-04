import React from "react";
import { BackButton } from "../../molecules/BackButton";
import { Title, Subtitle } from "../../molecules/PageHeadings";
import { DetailButton } from "./DetailButton";

export const Top = ({ integratedSystem }) => {
  if (!integratedSystem || integratedSystem.loading) {
    return (
      <React.Fragment>
        <BackButton to="/settings/integrations" className="float-right">
          Integrations
        </BackButton>
        <Title>Integrated System</Title>
        <Subtitle>...</Subtitle>
      </React.Fragment>
    );
  }
  return (
    <React.Fragment>
      <BackButton to="/settings/integrations" className="float-right">
        Integrations
      </BackButton>
      <DetailButton
        className="float-right"
        integratedSystem={integratedSystem}
      />
      <Title>Integrated System</Title>
      <Subtitle>{integratedSystem.name || integratedSystem.name}</Subtitle>
    </React.Fragment>
  );
};
