import React from "react";
import { useHistory } from "react-router-dom";
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

export const CreateChannel = () => {
  const [newChannel, setNewChannel] = React.useState({
    name: "",
    type: "",
    integratedSystemId: -1,
  });

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
        navigate({ pathname: `/channels` }); // TODO: navigate to details
      })
      .catch((e) => {
        analytics.track("site:channel_create_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };

  return (
    <React.Fragment>
      <div>
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
          <div>Choose an integrated system</div>
          <AsyncSelectIntegratedSystem
            onChange={(v) => setSelectedIntegratedSystem(v.value)}
          />
          {selectedIntegratedSystem &&
            selectedIntegratedSystem.systemType !== "custom" && (
              <div className="mt-3">
                <NoteBox label="Error" cardTitleClassName="text-danger">
                  Unsupported Integrated System Type
                </NoteBox>
              </div>
            )}
          {selectedIntegratedSystem &&
            selectedIntegratedSystem.systemType === "custom" && (
              <div className="mt-3">
                <NoteBox label="Info">
                  You are about to create a Webhook Channel
                </NoteBox>
              </div>
            )}
        </div>
        <div className="mt-4">
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
        </div>
      </div>
    </React.Fragment>
  );
};
