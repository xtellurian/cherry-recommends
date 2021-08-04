import React from "react";
import { useRouteMatch } from "react-router-dom";
import {
  useTrackedUser,
  useTrackedUserUniqueActions,
  useTrackedUserAction,
} from "../../api-hooks/trackedUserApi";
import { useUserEvents } from "../../api-hooks/eventApi";
import { BackButton } from "../molecules/BackButton";
import {
  Subtitle,
  Title,
  ErrorCard,
  Spinner,
  ExpandableCard,
  EmptyList,
} from "../molecules";
import { DateTimeField } from "../molecules/DateTimeField";
import {
  ActionsButton,
  ActionItemsGroup,
  ActionLink,
} from "../molecules/ActionsButton";
import { JsonView } from "../molecules/JsonView";
import { EventTimelineChart } from "../molecules/EventTimelineChart";

const ActionSubRow = ({ id, actionName }) => {
  const action = useTrackedUserAction({ id, actionName });
  const data = {
    category: action.category,
    eventId: action.eventId,
    actionName: action.actionName,
    actionValue: action.actionValue,
  };
  return (
    <div>
      {action.loading && <Spinner />}
      {action.timestamp && <DateTimeField label="Timestamp" date={action.timestamp} />}
      {data.actionName && <JsonView data={data} />}
    </div>
  );
};
const ActionRow = ({ id, actionName }) => {
  return (
    <ExpandableCard label={actionName}>
      <ActionSubRow id={id} actionName={actionName} />
    </ExpandableCard>
  );
};
export const TrackedUserDetail = () => {
  const { params } = useRouteMatch();
  const id = params["id"];
  const trackedUser = useTrackedUser({ id });
  const actions = useTrackedUserUniqueActions({ id });
  const events = useUserEvents({ commonUserId: trackedUser?.commonUserId });

  return (
    <React.Fragment>
      <ActionsButton
        className="ml-1 float-right"
        to={`/tracked-users/features/${id}`}
        label="Features"
      >
        <ActionItemsGroup label="Actions">
          <ActionLink
            to={`/tracked-users/touchpoints/${trackedUser.id}`}
          >
            Touchpoints
          </ActionLink>
          <ActionLink
            to={`/tracked-users/link-to-integrated-system/${trackedUser.id}`}
          >
            Link to Integrated System
          </ActionLink>
        </ActionItemsGroup>
      </ActionsButton>

      <BackButton className="float-right" to="/tracked-users">
        All Users
      </BackButton>
      <Title>Tracked User</Title>
      <Subtitle>{trackedUser.name || trackedUser.commonUserId}</Subtitle>
      <hr />
      {trackedUser.loading && <Spinner />}
      {trackedUser.error && <ErrorCard error={trackedUser.error} />}
      <ExpandableCard label="Details">
        <JsonView data={trackedUser} />
      </ExpandableCard>
      <div className="mt-2">
        <Subtitle>Latest Activity</Subtitle>
        {actions.loading && <Spinner />}
        {actions.error && <ErrorCard error={actions.error} />}
        {actions.actionNames && actions.actionNames.length === 0 && (
          <EmptyList>No Action Data</EmptyList>
        )}
        {actions.actionNames &&
          actions.actionNames.map((a) => (
            <ActionRow key={a} id={id} actionName={a} />
          ))}
      </div>
      <hr />
      <div className="mb-5">
        <h4>Events</h4>
        <EventTimelineChart eventResponse={events} />
      </div>
    </React.Fragment>
  );
};
