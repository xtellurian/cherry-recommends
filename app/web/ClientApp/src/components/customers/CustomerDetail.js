import React from "react";
import { Link, useParams } from "react-router-dom";
import { useCustomer } from "../../api-hooks/customersApi";
import { BackButton } from "../molecules/BackButton";
import { Subtitle, Title, ErrorCard, Spinner, EmptyList } from "../molecules";
import { DateTimeField } from "../molecules/DateTimeField";
import { CopyableField } from "../molecules/fields/CopyableField";
import { Tabs, TabActivator } from "../molecules/layout/Tabs";
import {
  ActionsButton,
  ActionItemsGroup,
  ActionLink,
} from "../molecules/buttons/ActionsButton";
import { JsonView } from "../molecules/JsonView";

import { HistorySection } from "./HistorySection";
import { LatestRecommendationsSection } from "./LatestRecommendationsSection";

const tabs = [
  {
    id: "properties",
    label: "Current Properties",
  },
  {
    id: "history",
    label: "Event History",
  },
  {
    id: "latest-recommendations",
    label: "Recommendations",
  },
];
const defaultTabId = tabs[0].id;

export const CustomerDetail = () => {
  const { id } = useParams();
  const trackedUser = useCustomer({ id });

  const numProperties = Object.keys(trackedUser?.properties || {}).length;
  return (
    <React.Fragment>
      <ActionsButton
        className="ml-1 float-right"
        to={`/customers/features/${id}`}
        label="Features"
      >
        <ActionItemsGroup label="Actions">
          <ActionLink to={`/customers/edit-properties/${id}`}>
            Edit Properties
          </ActionLink>
          <ActionLink to={`/customers/create-event/${id}`}>
            Log Event
          </ActionLink>
          <ActionLink to={`/customers/link-to-integrated-system/${id}`}>
            Link to Integrated System
          </ActionLink>
        </ActionItemsGroup>
      </ActionsButton>

      <BackButton className="float-right" to="/customers">
        All Customers
      </BackButton>
      <Title>Customer</Title>
      <Subtitle>{trackedUser.name || trackedUser.customerId}</Subtitle>
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

        <Link to={`/customers/edit-properties/${trackedUser.id}`}>
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
      <TabActivator
        tabId={"latest-recommendations"}
        defaultTabId={defaultTabId}
      >
        <LatestRecommendationsSection trackedUser={trackedUser} />
      </TabActivator>
    </React.Fragment>
  );
};
