import React from "react";
import { Subtitle, Title } from "../../../molecules/PageHeadings";
import { Spinner } from "../../../molecules/Spinner";
import { useHubspotClientAllContactProperties } from "../../../../api-hooks/hubspotApi";
import { ErrorCard } from "../../../molecules/ErrorCard";
import { ExpandableCard } from "../../../molecules/ExpandableCard";
import { JsonView } from "../../../molecules/JsonView";
import { BackButton } from "../../../molecules/BackButton";

const Top = () => {
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/settings/integrations">
        Integrations
      </BackButton>
      <Title> Hubspot Contact Properties</Title>
      <Subtitle>Available Contact Properties</Subtitle>
    </React.Fragment>
  );
};

const PropertyRow = ({ property }) => {
  return (
    <ExpandableCard label={property.label}>
      <JsonView data={property} />
    </ExpandableCard>
  );
};

export const HubspotContactProperties = ({ integratedSystem }) => {
  const properties = useHubspotClientAllContactProperties({ id: integratedSystem.id });
  return (
    <React.Fragment>
      <Top />
      <hr />
      {integratedSystem.loading && <Spinner>Loading Integrated System</Spinner>}
      {properties.loading && <Spinner>Loading Hubspot Properties</Spinner>}
      {properties.error && <ErrorCard error={properties.error} />}
      {properties &&
        properties.length &&
        properties.length > 0 &&
        properties.map((p) => <PropertyRow key={p.name} property={p} />)}
    </React.Fragment>
  );
};
