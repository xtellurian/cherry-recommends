import React from "react";
import { useParams } from "react-router-dom";

import { useSegment } from "../../api-hooks/segmentsApi";
import { useAccessToken } from "../../api-hooks/token";
import { deleteSegmentAsync } from "../../api/segmentsApi";
import { ErrorCard, Spinner, MoveUpHierarchyPrimaryButton } from "../molecules";
import { Tabs, TabActivator } from "../molecules/layout/Tabs";
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";
import { MembersSection } from "./MembersSection";
import { PageHeading } from "../molecules/layout/PageHeadings";
import { SegmentEnrolmentSection } from "./SegmentEnrolmentSection";
import { useNavigation } from "../../utility/useNavigation";
import EntityDetailPageLayout from "../molecules/layout/EntityDetailPageLayout";

const tabs = [
  // {
  //   id: "details",
  //   label: "Details",
  // },
  {
    id: "members",
    label: "Members",
  },
  {
    id: "enrolment",
    label: "Enrolment",
  },
];
const defaultTabId = tabs[0].id;

export const SegmentDetail = () => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const { id } = useParams();
  const segment = useSegment({ id });

  const [isDeletePopupOpen, setIsDeletePopupOpen] = React.useState(false);
  const [error, setError] = React.useState();

  return (
    <EntityDetailPageLayout
      backButton={
        <MoveUpHierarchyPrimaryButton
          to={{ pathname: "/customers/segments", search: null }}
        >
          Back to Segments
        </MoveUpHierarchyPrimaryButton>
      }
      header={<PageHeading title={segment.name || "..."} subtitle="Segment" />}
      options={
        <button
          className="btn btn-danger ml-1"
          onClick={() => setIsDeletePopupOpen(true)}
        >
          Delete Segment
        </button>
      }
    >
      {segment.loading && <Spinner />}
      {segment.error && <ErrorCard error={segment.error} />}
      <Tabs tabs={tabs} defaultTabId={tabs[0].id} />
      <TabActivator
        tabId={"details"}
        defaultTabId={defaultTabId}
      ></TabActivator>
      <TabActivator tabId={"members"} defaultTabId={defaultTabId}>
        <MembersSection segment={segment} />
      </TabActivator>
      <TabActivator tabId={"enrolment"} defaultTabId={defaultTabId}>
        <SegmentEnrolmentSection id={id} />
      </TabActivator>
      <ConfirmDeletePopup
        entity={segment}
        error={error}
        open={isDeletePopupOpen}
        setOpen={setIsDeletePopupOpen}
        handleDelete={() =>
          deleteSegmentAsync({ id, token })
            .then(() => navigate("/customers/segments"))
            .catch(setError)
        }
      />
    </EntityDetailPageLayout>
  );
};
