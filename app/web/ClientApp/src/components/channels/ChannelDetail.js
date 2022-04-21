import React from "react";
import { useParams } from "react-router-dom";
import { useChannel } from "../../api-hooks/channelsApi";
import { useAccessToken } from "../../api-hooks/token";
import { deleteChannelAsync } from "../../api/channelsApi";
import {
  PageHeading,
  ErrorCard,
  Spinner,
  MoveUpHierarchyPrimaryButton,
} from "../molecules";
import { DateTimeField } from "../molecules/DateTimeField";
import { CopyableField } from "../molecules/fields/CopyableField";
import { Tabs, TabActivator } from "../molecules/layout/Tabs";
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";
import { useNavigation } from "../../utility/useNavigation";
import { ConfigureChannel } from "./ConfigureChannel";

const tabs = [
  {
    id: "summary",
    label: "Summary",
  },
  {
    id: "configure",
    label: "Configure",
  },
];
const defaultTabId = tabs[0].id;

const channelTypeLabels = {
  webhook: "Webhook",
  email: "Email",
  web: "Web",
};

export const ChannelDetail = () => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const { id } = useParams();
  const channel = useChannel({ id });
  const [isDeletePopupOpen, setIsDeletePopupOpen] = React.useState(false);
  const [error, setError] = React.useState();

  return (
    <React.Fragment>
      <MoveUpHierarchyPrimaryButton
        to={{ pathname: "/channels", search: null }}
      >
        Back to Channels
      </MoveUpHierarchyPrimaryButton>

      <button
        className="btn btn-danger ml-1 float-right"
        onClick={() => setIsDeletePopupOpen(true)}
      >
        Delete Channel
      </button>

      <PageHeading
        title={channel.name || "..."}
        subtitle="Channel"
        showHr={false}
      />

      {channel.loading && <Spinner />}
      {channel.error && <ErrorCard error={channel.error} />}

      <Tabs tabs={tabs} defaultTabId={tabs[0].id} />

      <TabActivator tabId={tabs[0].id} defaultTabId={defaultTabId}>
        {channel.name ? (
          <CopyableField label="Channel Name" value={channel.name} />
        ) : null}

        {channel.channelType ? (
          <CopyableField
            label="Channel Type"
            value={channelTypeLabels[channel.channelType]}
          />
        ) : null}

        {channel.linkedIntegratedSystem?.name ? (
          <CopyableField
            label="Integrated System Name"
            value={channel.linkedIntegratedSystem?.name}
          />
        ) : null}

        {channel.lastCompleted ? (
          <DateTimeField label="Last Completed" date={channel.lastCompleted} />
        ) : null}

        {channel.lastEnqueued ? (
          <DateTimeField label="Last Enqueued" date={channel.lastEnqueued} />
        ) : null}
      </TabActivator>

      <TabActivator tabId="configure" defaultTabId={defaultTabId}>
        <ConfigureChannel channel={channel}></ConfigureChannel>
      </TabActivator>

      <ConfirmDeletePopup
        entity={channel}
        error={error}
        open={isDeletePopupOpen}
        setOpen={setIsDeletePopupOpen}
        handleDelete={() =>
          deleteChannelAsync({ id, token })
            .then(() => navigate({ pathname: "/channels" }))
            .catch(setError)
        }
      />
    </React.Fragment>
  );
};
