import React from "react";
import { Link, useParams, useHistory } from "react-router-dom";
import { useCustomer } from "../../api-hooks/customersApi";
import { useAccessToken } from "../../api-hooks/token";
import { deleteCustomerAsync } from "../../api/customersApi";
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
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";

import { HistorySection } from "./HistorySection";
import { LatestRecommendationsSection } from "./LatestRecommendationsSection";

const tabs = [
  {
    id: "history",
    label: "Event History",
  },
  {
    id: "latest-recommendations",
    label: "Recommendations",
  },
  {
    id: "properties",
    label: "Current Properties",
  },
];
const defaultTabId = tabs[0].id;

export const CustomerDetail = () => {
  const token = useAccessToken();
  const history = useHistory();
  const { id } = useParams();
  const trackedUser = useCustomer({ id });

  const [isDeletePopupOpen, setIsDeletePopupOpen] = React.useState(false);
  const [error, setError] = React.useState();

  const numProperties = Object.keys(trackedUser?.properties || {}).length;

  return (
    <React.Fragment>
      <ActionsButton
        className="ml-1 float-right"
        to={`/customers/metrics/${id}`}
        label="Metrics"
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
      <button
        className="btn btn-danger ml-1 float-right"
        onClick={() => setIsDeletePopupOpen(true)}
      >
        Delete Customer
      </button>
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
      {trackedUser.email && (
        <CopyableField label="Email" value={trackedUser.email} />
      )}

      <Tabs tabs={tabs} defaultTabId={tabs[0].id} />

      <TabActivator tabId="properties" defaultTabId={defaultTabId}>
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

      <ConfirmDeletePopup
        entity={trackedUser}
        error={error}
        open={isDeletePopupOpen}
        setOpen={setIsDeletePopupOpen}
        handleDelete={() =>
          deleteCustomerAsync({ id, token })
            .then(() => history.push("/customers"))
            .catch(setError)
        }
      />
    </React.Fragment>
  );
};
