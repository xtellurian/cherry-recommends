import React from "react";
import {
  BackButton,
  EmptyList,
  ExpandableCard,
  Subtitle,
  Title,
} from "../molecules";
import { useEvent } from "../../api-hooks/eventApi";
import { useCustomer } from "../../api-hooks/customersApi";
import { useParams } from "react-router-dom";
import { DateTimeField } from "../molecules/DateTimeField";
import { CopyableField } from "../molecules/fields/CopyableField";
import { EntityField } from "../molecules/EntityField";
import { JsonView } from "../molecules/JsonView";

export const EventDetail = () => {
  const { id } = useParams();
  const eventInfo = useEvent({ id });
  const trackedUser = useCustomer({ id: eventInfo.trackedUserId });
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/">
        Home
      </BackButton>
      <Title>Event Detail</Title>
      <Subtitle>{id}</Subtitle>
      <hr />
      <DateTimeField date={eventInfo.timestamp} label="Timestamp" />
      <EntityField
        label="Customer"
        entity={trackedUser}
        to={`/customers/detail/${eventInfo.trackedUserId}`}
      />
      <CopyableField label="Event ID" value={eventInfo.eventId} />
      <CopyableField label="Kind" value={eventInfo.kind} />
      <CopyableField label="Event Type" value={eventInfo.eventType} />
      <CopyableField
        label="Recommendation Correlator"
        value={eventInfo.recommendationCorrelatorId}
      />
      <hr />
      <ExpandableCard label="Properties">
        <JsonView data={eventInfo.properties} />
      </ExpandableCard>
    </React.Fragment>
  );
};
