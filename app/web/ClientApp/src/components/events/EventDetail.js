import React from "react";
import { BackButton, ExpandableCard, Subtitle, Title } from "../molecules";
import { useEvent } from "../../api-hooks/eventApi";
import { useCustomer } from "../../api-hooks/customersApi";
import { useParams } from "react-router-dom";
import { DateTimeField } from "../molecules/DateTimeField";
import { CopyableField } from "../molecules/fields/CopyableField";
import { EntityField } from "../molecules/EntityField";
import { JsonView } from "../molecules/JsonView";
import { StatefulTabs, TabActivator } from "../molecules/layout/StatefulTabs";

const tabs = [
  { id: "detail", label: "Detail" },
  { id: "raw", label: "Raw" },
];
export const EventDetail = ({ event }) => {
  const trackedUser = useCustomer({ id: event.trackedUserId });
  const [currentTabId, setCurrentTabId] = React.useState(tabs[0].id);
  return (
    <React.Fragment>
      <Title>Event Detail</Title>
      <Subtitle>{event.eventId}</Subtitle>
      <StatefulTabs
        tabs={tabs}
        currentTabId={currentTabId}
        setCurrentTabId={setCurrentTabId}
      />
      <TabActivator currentTabId={currentTabId} tabId="detail">
        <DateTimeField date={event.timestamp} label="Timestamp" />
        <EntityField
          label="Customer"
          entity={trackedUser}
          to={`/customers/detail/${event.trackedUserId}`}
        />
        <CopyableField label="Event ID" value={event.eventId} />
        <CopyableField label="Kind" value={event.eventKind} />
        <CopyableField label="Event Type" value={event.eventType} />
        <CopyableField
          label="Recommendation Correlator"
          value={event.recommendationCorrelatorId}
        />
        <hr />
        <ExpandableCard label="Properties">
          <JsonView data={event.properties} />
        </ExpandableCard>
      </TabActivator>

      <TabActivator currentTabId={currentTabId} tabId="raw">
        <JsonView data={event} />
      </TabActivator>
    </React.Fragment>
  );
};
export const EventDetailPage = () => {
  const { id } = useParams();
  const event = useEvent({ id });
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/">
        Home
      </BackButton>
      <EventDetail event={event} />;
    </React.Fragment>
  );
};
