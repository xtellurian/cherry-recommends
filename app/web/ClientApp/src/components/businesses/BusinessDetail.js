import React from "react";
import { useParams } from "react-router-dom";

import { useBusiness } from "../../api-hooks/businessesApi";
import { useAccessToken } from "../../api-hooks/token";
import { deleteBusinessAsync } from "../../api/businessesApi";
import {
  Subtitle,
  PageHeading,
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
import { MembersSection } from "./MembersSection";
import { BusinessEventsSection } from "./BusinessEventsSection";
import { Recommendations } from "./Recommendations";
import { useNavigation } from "../../utility/useNavigation";
import EntityDetailPageLayout from "../molecules/layout/EntityDetailPageLayout";

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
    id: "recommendations",
    label: "Recommendations",
  },
  {
    id: "members",
    label: "Members",
  },
];
const defaultTabId = tabs[0].id;

export const BusinessDetail = () => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const { id } = useParams();
  const business = useBusiness({ id });

  const [isDeletePopupOpen, setIsDeletePopupOpen] = React.useState(false);
  const [error, setError] = React.useState();

  const numProperties = Object.keys(business?.properties || {}).length;

  return (
    <EntityDetailPageLayout
      backButton={
        <MoveUpHierarchyPrimaryButton
          to={{ pathname: "/customers/businesses", search: null }}
        >
          Back to Businesses
        </MoveUpHierarchyPrimaryButton>
      }
      header={
        <PageHeading
          title={business.name || business.commonId || "..."}
          subtitle="Business"
        />
      }
      options={
        <>
          <ActionsButton
            className="ml-1"
            to={`/customers/businesses/metrics/${id}`}
            label="Metrics"
          >
            <ActionItemsGroup label="Actions">
              <ActionLink to={`/customers/businesses/edit-properties/${id}`}>
                Edit Properties
              </ActionLink>
              <ActionLink to={`/customers/businesses/create-event/${id}`}>
                Log Event
              </ActionLink>
            </ActionItemsGroup>
          </ActionsButton>
          <button
            className="btn btn-danger ml-1"
            onClick={() => setIsDeletePopupOpen(true)}
          >
            Delete Business
          </button>
        </>
      }
    >
      {business.loading && <Spinner />}
      {business.error && <ErrorCard error={business.error} />}
      {business.lastUpdated && (
        <DateTimeField label="Last Updated" date={business.lastUpdated} />
      )}
      {business.commonId && (
        <CopyableField label="Common Id" value={business.commonId} />
      )}
      {business.description && (
        <CopyableField label="Description" value={business.description} />
      )}

      <Tabs tabs={tabs} defaultTabId={tabs[0].id} />

      <TabActivator tabId={tabs[0].id} defaultTabId={defaultTabId}>
        <Subtitle>Properties ({numProperties})</Subtitle>

        <Navigation to={`/customers/businesses/edit-properties/${business.id}`}>
          <button className="btn btn-outline-primary float-right">
            Edit Properties
          </button>
        </Navigation>
        {numProperties > 0 && <JsonView data={business.properties} />}
        {numProperties === 0 && (
          <EmptyList>Business has no properties.</EmptyList>
        )}
      </TabActivator>

      <TabActivator tabId={"history"} defaultTabId={defaultTabId}>
        <BusinessEventsSection business={business} />
      </TabActivator>
      <TabActivator tabId={"recommendations"} defaultTabId={defaultTabId}>
        <Recommendations business={business} />
      </TabActivator>
      <TabActivator tabId={"members"} defaultTabId={defaultTabId}>
        <MembersSection business={business} />
      </TabActivator>

      <ConfirmDeletePopup
        entity={business}
        error={error}
        open={isDeletePopupOpen}
        setOpen={setIsDeletePopupOpen}
        handleDelete={() =>
          deleteBusinessAsync({ id, token })
            .then(() => navigate("/customers/businesses"))
            .catch(setError)
        }
      />
    </EntityDetailPageLayout>
  );
};
