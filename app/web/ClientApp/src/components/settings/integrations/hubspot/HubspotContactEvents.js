import React from "react";
import { Subtitle, Title } from "../../../molecules/layout";
import { Spinner } from "../../../molecules/Spinner";
import { useHubspotContactEvents } from "../../../../api-hooks/hubspotApi";
import { ErrorCard } from "../../../molecules/ErrorCard";
import { ExpandableCard } from "../../../molecules/ExpandableCard";
import { JsonView } from "../../../molecules/JsonView";
import { EmptyList, MoveUpHierarchyButton } from "../../../molecules";

const Top = () => {
  return (
    <React.Fragment>
      <MoveUpHierarchyButton
        className="float-right"
        to="/settings/integrations"
      >
        Integrations
      </MoveUpHierarchyButton>
      <Title>Hubspot Contact Events</Title>
      <Subtitle>Sample Contact Events</Subtitle>
    </React.Fragment>
  );
};

const EventRow = ({ event }) => {
  return (
    <ExpandableCard label={event.eventType}>
      <JsonView data={event} />
    </ExpandableCard>
  );
};

export const HubspotContactEvents = ({ integratedSystem }) => {
  const contactEvents = useHubspotContactEvents({ id: integratedSystem.id });
  return (
    <React.Fragment>
      <Top />
      <hr />
      {integratedSystem.loading && <Spinner>Loading Integrated System</Spinner>}
      {contactEvents.loading && <Spinner>Loading Hubspot Events</Spinner>}
      {contactEvents.error && <ErrorCard error={contactEvents.error} />}
      {contactEvents &&
        contactEvents.length > 0 &&
        contactEvents.map((e, i) => <EventRow key={i} event={e} />)}
      {contactEvents.length === 0 && (
        <EmptyList>There are no contact events.</EmptyList>
      )}
    </React.Fragment>
  );
};
