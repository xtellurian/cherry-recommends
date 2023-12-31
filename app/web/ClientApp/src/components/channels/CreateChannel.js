import React, { useState, useEffect } from "react";
import { useAccessToken } from "../../api-hooks/token";
import { createChannelAsync } from "../../api/channelsApi";
import {
  AsyncButton,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
} from "../molecules";
import { TextInput } from "../molecules/TextInput";
import { useAnalytics } from "../../analytics/analyticsHooks";
import { AsyncSelectIntegratedSystem } from "../molecules/selectors/AsyncSelectIntegratedSystem";
import { useNavigation } from "../../utility/useNavigation";
import Select from "../molecules/selectors/Select";
import CreatePageLayout from "../molecules/layout/CreatePageLayout";

const channelTypeOptions = [
  { value: "webhook", label: "Webhook" },
  { value: "web", label: "Web" },
  { value: "email", label: "Email" },
];

const defaultChannel = {
  name: "",
  channelType: "webhook",
  integratedSystemId: -1,
};

export const CreateChannel = () => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const { analytics } = useAnalytics();
  const [error, setError] = useState();
  const [loading, setLoading] = useState(false);
  const [newChannel, setNewChannel] = useState(defaultChannel);
  const [integratedSystemType, setIntegratedSystemType] = useState(null);
  const [selectedIntegratedSystem, setSelectedIntegratedSystem] =
    useState(null);

  const setSelectedChannelType = (o) => {
    setNewChannel({ ...newChannel, channelType: o.value });
  };

  const handleCreate = () => {
    setLoading(true);
    createChannelAsync({
      channel: {
        ...newChannel,
        integratedSystemId: selectedIntegratedSystem.id,
      },
      token,
    })
      .then((u) => {
        analytics.track("site:channel_create_success");
        navigate({ pathname: `/integrations/channels/detail/${u.id}` });
      })
      .catch((e) => {
        analytics.track("site:channel_create_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    const integratedSystemTypes = {
      webhook: "custom",
      web: "website",
      email: "klaviyo",
    };

    setSelectedIntegratedSystem(null); // clears the selected integrated system when channelType changes
    setIntegratedSystemType(
      integratedSystemTypes[newChannel.channelType] || null
    );
  }, [newChannel.channelType]);

  return (
    <React.Fragment>
      <CreatePageLayout
        createButton={
          <AsyncButton
            loading={loading}
            className="btn btn-primary"
            onClick={handleCreate}
            disabled={
              !newChannel.name ||
              !newChannel.channelType ||
              !selectedIntegratedSystem
            }
          >
            Create
          </AsyncButton>
        }
        backButton={
          <MoveUpHierarchyPrimaryButton to="/integrations/channels">
            Back to Channels
          </MoveUpHierarchyPrimaryButton>
        }
        header={<PageHeading title="Add a Channel" />}
        error={error}
      >
        <TextInput
          label="Name"
          placeholder="Type the name of your channel"
          value={newChannel.name}
          onChange={(e) =>
            setNewChannel({
              ...newChannel,
              name: e.target.value,
            })
          }
        />

        <div>
          <Select
            label="Channel Type"
            className="mb-1"
            placeholder="Select channel type"
            onChange={setSelectedChannelType}
            options={channelTypeOptions}
            defaultValue={channelTypeOptions[0]}
          />
        </div>
        <div>
          <AsyncSelectIntegratedSystem
            label="Connection"
            value={selectedIntegratedSystem}
            systemType={integratedSystemType}
            onChange={(v) => setSelectedIntegratedSystem(v.value)}
          />
        </div>
      </CreatePageLayout>
    </React.Fragment>
  );
};
