import React from "react";
import { Link, useParams, useHistory } from "react-router-dom";
import { useBusiness } from "../../api-hooks/businessesApi";
import { useAccessToken } from "../../api-hooks/token";
import { deleteBusinessAsync } from "../../api/businessesApi";
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
  {
    id: "members",
    label: "Members",
  },
];
const defaultTabId = tabs[0].id;

export const BusinessDetail = () => {
  const token = useAccessToken();
  const history = useHistory();
  const { id } = useParams();
  const business = useBusiness({ id });

  const [isDeletePopupOpen, setIsDeletePopupOpen] = React.useState(false);
  const [error, setError] = React.useState();

  const numProperties = Object.keys(business?.properties || {}).length;

  return (
    <React.Fragment>
      <ActionsButton
        className="ml-1 float-right"
        to={`/businesses/metrics/${id}`}
        label="Metrics"
      >
        <ActionItemsGroup label="Actions">
          <ActionLink to={`/businesses/edit-properties/${id}`}>
            Edit Properties
          </ActionLink>
          <ActionLink to={`/businesses/create-event/${id}`}>
            Log Event
          </ActionLink>
          <ActionLink to={`/businesses/link-to-integrated-system/${id}`}>
            Add Members
          </ActionLink>
        </ActionItemsGroup>
      </ActionsButton>
      <button
        className="btn btn-danger ml-1 float-right"
        onClick={() => setIsDeletePopupOpen(true)}
      >
        Delete Business
      </button>
      <BackButton className="float-right" to="/businesses">
        All Businesses
      </BackButton>
      <Title>Business</Title>
      <Subtitle>{business.name || business.commonId}</Subtitle>
      <hr />

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

        <Link to={`/businesses/edit-properties/${business.id}`}>
          <button className="btn btn-outline-primary float-right">
            Edit Properties
          </button>
        </Link>
        {numProperties > 0 && <JsonView data={business.properties} />}
        {numProperties === 0 && <EmptyList>Business has no properties.</EmptyList>}
      </TabActivator>

      <TabActivator tabId={"history"} defaultTabId={defaultTabId}>
      </TabActivator>
      <TabActivator tabId={"latest-recommendations"} defaultTabId={defaultTabId}>
      </TabActivator>
      <TabActivator tabId={"members"} defaultTabId={defaultTabId}>
      </TabActivator>

      <ConfirmDeletePopup
        entity={business}
        error={error}
        open={isDeletePopupOpen}
        setOpen={setIsDeletePopupOpen}
        handleDelete={() =>
          deleteBusinessAsync({ id, token })
            .then(() => history.push("/businesses"))
            .catch(setError)
        }
      />
    </React.Fragment>
  );
};
