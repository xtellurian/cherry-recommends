import React from "react";
import { Link, useParams } from "react-router-dom";
import { useTrackedUser } from "../../api-hooks/trackedUserApi";
import { BackButton } from "../molecules/BackButton";
import { Subtitle, Title, ErrorCard, Spinner, EmptyList } from "../molecules";
import { DateTimeField } from "../molecules/DateTimeField";
import { CopyableField } from "../molecules/CopyableField";
import { Tabs, TabActivator } from "../molecules/Tabs";
import {
  ActionsButton,
  ActionItemsGroup,
  ActionLink,
} from "../molecules/ActionsButton";
import { JsonView } from "../molecules/JsonView";

import { HistorySection } from "./HistorySection";

const tabs = [
  {
    id: "properties",
    label: "Properties",
  },
  {
    id: "history",
    label: "History",
  },
];
const defaultTabId = tabs[0].id;

export const TrackedUserDetail = () => {
  const { id } = useParams();
  const trackedUser = useTrackedUser({ id });

  const numProperties = Object.keys(trackedUser?.properties || {}).length;
  return (
    <React.Fragment>
      <ActionsButton
        className="ml-1 float-right"
        to={`/tracked-users/features/${id}`}
        label="Features"
      >
        <ActionItemsGroup label="Actions">
          <ActionLink to={`/tracked-users/edit-properties/${id}`}>
            Edit Properties
          </ActionLink>
          <ActionLink to={`/tracked-users/create-event/${id}`}>
            Log Event
          </ActionLink>
          <ActionLink to={`/tracked-users/touchpoints/${id}`}>
            Touchpoints
          </ActionLink>
          <ActionLink to={`/tracked-users/link-to-integrated-system/${id}`}>
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
      {trackedUser.lastUpdated && (
        <DateTimeField label="Last Updated" date={trackedUser.lastUpdated} />
      )}
      {trackedUser.commonId && (
        <CopyableField label="Common Id" value={trackedUser.commonId} />
      )}

      <Tabs tabs={tabs} defaultTabId={tabs[0].id} />

      <TabActivator tabId={tabs[0].id} defaultTabId={defaultTabId}>
        <Subtitle>Properties ({numProperties})</Subtitle>

        <Link to={`/tracked-users/edit-properties/${trackedUser.id}`}>
          <button className="btn btn-outline-primary float-right">
            Edit Properties
          </button>
        </Link>
        {numProperties > 0 && <JsonView data={trackedUser.properties} />}
        {numProperties === 0 && <EmptyList>User has no properties.</EmptyList>}
      </TabActivator>

      <TabActivator tabId={"history"} defaultTabId={defaultTabId}>
        <HistorySection trackedUser={trackedUser} />
      </TabActivator>
    </React.Fragment>
  );
};
