import React from "react";
import { useParams, useHistory } from "react-router-dom";
import { useSegment } from "../../api-hooks/segmentsApi";
import { useAccessToken } from "../../api-hooks/token";
import { deleteSegmentAsync } from "../../api/segmentsApi";
import { BackButton } from "../molecules/BackButton";
import { Subtitle, Title, ErrorCard, Spinner, EmptyList } from "../molecules";
import { Tabs, TabActivator } from "../molecules/layout/Tabs";

import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";
import { MembersSection } from "./MembersSection";
import { PageHeading } from "../molecules/layout/PageHeadings";

const tabs = [
  // {
  //   id: "details",
  //   label: "Details",
  // },
  {
    id: "members",
    label: "Members",
  },
  // {
  //   id: "enrolment",
  //   label: "Enrolment",
  // },
];
const defaultTabId = tabs[0].id;

export const SegmentDetail = () => {
  const token = useAccessToken();
  const history = useHistory();
  const { id } = useParams();
  const segment = useSegment({ id });

  const [isDeletePopupOpen, setIsDeletePopupOpen] = React.useState(false);
  const [error, setError] = React.useState();

  return (
    <React.Fragment>
      <button
        className="btn btn-danger ml-1 float-right"
        onClick={() => setIsDeletePopupOpen(true)}
      >
        Delete Segment
      </button>
      <BackButton className="float-right" to="/segments">
        All Segments
      </BackButton>
      <PageHeading title="Segment" subtitle={segment.name} />
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
      <TabActivator
        tabId={"enrolment"}
        defaultTabId={defaultTabId}
      ></TabActivator>

      <ConfirmDeletePopup
        entity={segment}
        error={error}
        open={isDeletePopupOpen}
        setOpen={setIsDeletePopupOpen}
        handleDelete={() =>
          deleteSegmentAsync({ id, token })
            .then(() => history.push("/segments"))
            .catch(setError)
        }
      />
    </React.Fragment>
  );
};