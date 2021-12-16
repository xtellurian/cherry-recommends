import React from "react";
import { Link } from "react-router-dom";
import { useCustomerEvents } from "../../api-hooks/eventApi";
import {
  Subtitle,
  Spinner,
  ExpandableCard,
  ErrorCard,
  EmptyList,
} from "../molecules";
import { DateTimeField } from "../molecules/DateTimeField";
import { JsonView } from "../molecules/JsonView";

const EventRow = ({ event }) => {
  return (
    <ExpandableCard label={`${event.kind} @ ${event.timestamp}`}>
      <div>
        {event.timestamp && (
          <DateTimeField label="Timestamp" date={event.timestamp} />
        )}
        <JsonView
          data={{
            kind: event.eventKind ?? event.kind,
            eventType: event.eventType,
            recommendationCorrelatorId: event.recommendationCorrelatorId,
            properties: event.properties,
          }}
        />
      </div>
    </ExpandableCard>
  );
};

const ViewAsEvents = ({ trackedUser }) => {
  const events = useCustomerEvents({ id: trackedUser?.id });

  return (
    <div className="m-2">
      <div className="mb-5">
        <Link to={`/customers/create-event/${trackedUser.id}`}>
          <button className="btn btn-outline-primary float-right">
            Log New Event
          </button>
        </Link>
        <Subtitle>Events</Subtitle>
      </div>
      {events.error && <ErrorCard error={events.error} />}
      {events.loading && <Spinner>Loading events</Spinner>}
      {events.items && events.items.length === 0 && (
        <EmptyList>No events</EmptyList>
      )}
      {events.items &&
        events.items.length > 0 &&
        events.items.map((e) => <EventRow key={e.eventId} event={e} />)}
    </div>
  );
};
export const HistorySection = ({ trackedUser }) => {
  return (
    <React.Fragment>
      {trackedUser.loading && <Spinner />}
      {trackedUser.id && (
        <div className="mb-5">
          <ViewAsEvents trackedUser={trackedUser} />
        </div>
      )}
    </React.Fragment>
  );
};
