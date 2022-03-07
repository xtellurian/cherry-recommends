import React from "react";
import { useBusinessEvents } from "../../api-hooks/eventApi";
import { Subtitle, Spinner, ErrorCard, EmptyList } from "../molecules";

import { EventRow } from "../events/EventRow";

const ViewAsEvents = ({ business }) => {
  const events = useBusinessEvents({ id: business?.id });

  return (
    <div className="m-2">
      <div className="mb-5">
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

export const BusinessEventsSection = ({ business }) => {
  return (
    <React.Fragment>
      {business.loading && <Spinner />}
      {business.id && (
        <div className="mb-5">
          <ViewAsEvents business={business} />
        </div>
      )}
    </React.Fragment>
  );
};
