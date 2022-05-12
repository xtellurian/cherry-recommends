import React from "react";
import { useParams } from "react-router-dom";

import { useCustomer } from "../../api-hooks/customersApi";
import { useAccessToken } from "../../api-hooks/token";
import { deleteCustomerAsync } from "../../api/customersApi";
import {
  PageHeading,
  Subtitle,
  ErrorCard,
  Spinner,
  EmptyList,
  Navigation,
  MoveUpHierarchyPrimaryButton,
} from "../molecules";
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
import { useNavigation } from "../../utility/useNavigation";
import EntityDetailPageLayout from "../molecules/layout/EntityDetailPageLayout";

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
  const { navigate } = useNavigation();
  const { id } = useParams();
  const trackedUser = useCustomer({ id });

  const [isDeletePopupOpen, setIsDeletePopupOpen] = React.useState(false);
  const [error, setError] = React.useState();

  const numProperties = Object.keys(trackedUser?.properties || {}).length;

  return (
    <EntityDetailPageLayout
      backButton={
        <MoveUpHierarchyPrimaryButton
          to={{ pathname: "/customers", search: null }}
        >
          Back to Customers
        </MoveUpHierarchyPrimaryButton>
      }
      header={
        <PageHeading
          title={trackedUser.name || trackedUser.customerId || "..."}
          subtitle="Customer"
        />
      }
      options={
        <>
          <ActionsButton
            className="ml-1"
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
            className="btn btn-danger ml-1"
            onClick={() => setIsDeletePopupOpen(true)}
          >
            Delete Customer
          </button>
        </>
      }
    >
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

        <Navigation to={`/customers/edit-properties/${trackedUser.id}`}>
          <button className="btn btn-outline-primary float-right">
            Edit Properties
          </button>
        </Navigation>
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
            .then(() => navigate("/customers"))
            .catch(setError)
        }
      />
    </EntityDetailPageLayout>
  );
};
