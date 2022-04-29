import React, { useState, useEffect } from "react";
import { useAccessToken } from "../../api-hooks/token";
import { createChannelAsync } from "../../api/channelsApi";
import { Title } from "../molecules/layout";
import { ErrorCard } from "../molecules/ErrorCard";
import { AsyncButton } from "../molecules";
import { InputGroup, TextInput } from "../molecules/TextInput";
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
        navigate({ pathname: `/channels/detail/${u.id}` });
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
      >
        <Title>Add a Channel</Title>
        <hr />
        {error && <ErrorCard error={error} />}
        <InputGroup className="mb-1">
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
        </InputGroup>
        <div>
          <div>Choose a type</div>
          <Select
            className="mb-1"
            placeholder="Select channel type"
            onChange={setSelectedChannelType}
            options={channelTypeOptions}
            defaultValue={channelTypeOptions[0]}
          />
        </div>
        <div>
          <div>Choose an integrated system</div>
          <AsyncSelectIntegratedSystem
            value={selectedIntegratedSystem}
            systemType={integratedSystemType}
            onChange={(v) => setSelectedIntegratedSystem(v.value)}
          />
        </div>
      </CreatePageLayout>
    </React.Fragment>
  );
};
