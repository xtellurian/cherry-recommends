import React from "react";
import { BackButton, EmptyList, Subtitle, Title } from "../molecules";
import { useEvent } from "../../api-hooks/eventApi";
import { useTrackedUser } from "../../api-hooks/trackedUserApi";
import { useParams } from "react-router-dom";
import { DateTimeField } from "../molecules/DateTimeField";
import { CopyableField } from "../molecules/fields/CopyableField";
import { EntityField } from "../molecules/EntityField";

const ActionRow = ({ action }) => {
  return (
    <div className="row">
      <div className="col">
        <DateTimeField date={action.timestamp} label="Ocurred At" />
      </div>
      <div className="col">Category: {action.category}</div>
      <div className="col">Revenue: {action.associatedRevenue || "None"}</div>
      <div className="col-2">Value: {action.stringValue || "None"}</div>
    </div>
  );
};
export const EventDetail = () => {
  const { id } = useParams();
  const eventInfo = useEvent({ id });
  const trackedUser = useTrackedUser({ id: eventInfo.trackedUserId });
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
        label="Tracked User"
        entity={trackedUser}
        to={`/tracked-users/detail/${eventInfo.commonUserId}`}
      />
      <CopyableField label="Event ID" value={eventInfo.eventId} />
      <CopyableField label="Kind" value={eventInfo.kind} />
      <CopyableField label="Event Type" value={eventInfo.eventType} />
      <hr />
      <h6>Actions</h6>
      {eventInfo.actions && eventInfo.actions.length === 0 && (
        <EmptyList>No Associated Action </EmptyList>
      )}

      {eventInfo.actions &&
        eventInfo.actions.map((a) => <ActionRow key={a.id} action={a} />)}d
    </React.Fragment>
  );
};
