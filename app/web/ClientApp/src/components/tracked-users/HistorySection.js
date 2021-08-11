import React from "react";
import { Link } from "react-router-dom";
import { useUserEvents } from "../../api-hooks/eventApi";
import {
  Subtitle,
  Spinner,
  ExpandableCard,
  ErrorCard,
  EmptyList,
} from "../molecules";
import { DateTimeField } from "../molecules/DateTimeField";
import { ToggleSwitch } from "../molecules/ToggleSwitch";
import { JsonView } from "../molecules/JsonView";
import { EventTimelineChart } from "../molecules/EventTimelineChart";
import {
  useTrackedUserUniqueActionGroups,
  useTrackedUserAction,
} from "../../api-hooks/trackedUserApi";

const ActionSubRow = ({ id, category, actionName }) => {
  const action = useTrackedUserAction({ id, category, actionName });
  if (action.error) {
    return <ErrorCard error={action.error} />;
  }
  const data = {
    category: action.category,
    eventId: action.eventId,
    actionName: action.actionName,
    actionValue: action.actionValue,
  };
  if (action.associatedRevenue) {
    data.associatedRevenue = action.associatedRevenue;
  }
  return (
    <div>
      {action.loading && <Spinner />}
      {action.timestamp && (
        <DateTimeField label="Timestamp" date={action.timestamp} />
      )}
      {data.actionName && <JsonView data={data} />}
    </div>
  );
};
const ActionGroupRow = ({ id, actionGroup }) => {
  return (
    <ExpandableCard label={`${actionGroup.category}|${actionGroup.actionName}`}>
      <ActionSubRow
        id={id}
        category={actionGroup.category}
        actionName={actionGroup.actionName}
      />
    </ExpandableCard>
  );
};

const EventRow = ({ event }) => {
  return (
    <ExpandableCard label={event.kind}>
      <div>
        {event.timestamp && (
          <DateTimeField label="Timestamp" date={event.timestamp} />
        )}
        <JsonView
          data={{
            kind: event.kind,
            eventType: event.eventType,
            properties: event.properties,
          }}
        />
      </div>
    </ExpandableCard>
  );
};

const ViewAsActions = ({ trackedUser }) => {
  const actionGroups = useTrackedUserUniqueActionGroups({ id: trackedUser.id });
  return (
    <div className="m-2">
      <Subtitle>Actions</Subtitle>
      {actionGroups.loading && <Spinner />}
      {actionGroups.error && <ErrorCard error={actionGroups.error} />}
      {actionGroups.items && actionGroups.items.length === 0 && (
        <EmptyList>No Action Data</EmptyList>
      )}
      {actionGroups.items &&
        actionGroups.items.map((a) => (
          <ActionGroupRow key={a} id={trackedUser.id} actionGroup={a} />
        ))}
    </div>
  );
};

const ViewAsEvents = ({ trackedUser }) => {
  const events = useUserEvents({ commonUserId: trackedUser?.commonUserId });

  return (
    <div className="m-2">
      <div className="mb-5">
        <Link to={`/tracked-users/create-event/${trackedUser.id}`}>
          <button className="btn btn-outline-primary float-right">
            Log New Event
          </button>
        </Link>
        <Subtitle>Events</Subtitle>
      </div>
      {events.loading && <Spinner>Loading events</Spinner>}
      {events.events &&
        events.events.length > 0 &&
        events.events.map((e) => <EventRow key={e.eventId} event={e} />)}
      <div className="mt-2 text-center">
        <strong>Event Timeline</strong>
        <EventTimelineChart eventResponse={events} />
      </div>
    </div>
  );
};
export const HistorySection = ({ trackedUser }) => {
  const [viewAsActions, setViewAsActions] = React.useState(false);

  return (
    <React.Fragment>
      <div className="p-1 text-right bg-light border-rounded">
        <span className="mr-3">View as single actions?</span>
        <ToggleSwitch
          id="view-as-actions"
          name="View as Actions"
          onChange={setViewAsActions}
          checked={viewAsActions}
        />
      </div>
      <div className="mb-5">
        {!viewAsActions && <ViewAsEvents trackedUser={trackedUser} />}
        {viewAsActions && <ViewAsActions trackedUser={trackedUser} />}
      </div>
    </React.Fragment>
  );
};
