import React from "react";
import { useAccessToken } from "../../api-hooks/token";
import { createChannelAsync } from "../../api/channelsApi";
import { Title } from "../molecules/layout";
import { ErrorCard } from "../molecules/ErrorCard";
import { AsyncButton } from "../molecules";
import { InputGroup, TextInput } from "../molecules/TextInput";
import { useAnalytics } from "../../analytics/analyticsHooks";
import { AsyncSelectIntegratedSystem } from "../molecules/selectors/AsyncSelectIntegratedSystem";
import { NoteBox } from "../molecules/NoteBox";
import { useNavigation } from "../../utility/useNavigation";
import Select from "../molecules/selectors/Select";
import CreatePageLayout from "../molecules/layout/CreatePageLayout";

export const CreateChannel = () => {
  const [newChannel, setNewChannel] = React.useState({
    name: "",
    type: "webhook",
    integratedSystemId: -1,
  });

  const channelTypeOptions = [{ value: "webhook", label: "Webhook" }];
  const setSelectedChannelType = (o) => {
    setNewChannel({ ...newChannel, type: o.value });
  };

  const [systemType, setSystemType] = React.useState();
  const [selectedIntegratedSystem, setSelectedIntegratedSystem] =
    React.useState();

  React.useEffect(() => {
    if (selectedIntegratedSystem) {
      if (selectedIntegratedSystem.systemType === "custom") {
        setNewChannel({
          ...newChannel,
          type: "webhook",
          integratedSystemId: selectedIntegratedSystem.id,
        });
      }
    }
  }, [selectedIntegratedSystem]);

  React.useEffect(() => {
    if (newChannel.type === "webhook") {
      setSystemType("custom");
    } else {
      setSystemType(null);
    }
  }, [newChannel.type]);

  const token = useAccessToken();
  const { navigate } = useNavigation();
  const { analytics } = useAnalytics();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);

  const handleCreate = () => {
    setLoading(true);
    createChannelAsync({
      channel: newChannel,
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

  return (
    <React.Fragment>
      <CreatePageLayout
        createButton={
          <AsyncButton
            loading={loading}
            className="btn btn-primary"
            onClick={handleCreate}
            disabled={
              !selectedIntegratedSystem ||
              selectedIntegratedSystem.systemType !== "custom"
            }
          >
            Create
          </AsyncButton>
        }
      >
        <Title>Add a Channel</Title>
        <hr />
        {error && <ErrorCard error={error} />}
        <div className="text-warning">
          Currently supports Webhook channel type only.
        </div>
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
            systemType={systemType}
            onChange={(v) => setSelectedIntegratedSystem(v.value)}
          />
        </div>
      </CreatePageLayout>
    </React.Fragment>
  );
};
