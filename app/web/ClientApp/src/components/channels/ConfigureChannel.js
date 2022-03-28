import React, { useEffect } from "react";
import { useAccessToken } from "../../api-hooks/token";
import { updateChannelEndpointAsync } from "../../api/channelsApi";
import { CopyableField } from "../molecules/fields/CopyableField";
import { useNavigation } from "../../utility/useNavigation";
import {
  InputGroup,
  TextInput,
  createStartsWithValidator,
} from "../molecules/TextInput";
import { AsyncButton } from "../molecules";
import { end } from "@popperjs/core";

export const ConfigureChannel = ({ channel }) => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const [error, setError] = React.useState();
  const [saving, setSaving] = React.useState(false);
  const [endpoint, setEndpoint] = React.useState("");

  console.log(endpoint);
  console.log(channel.endpoint);
  const handleSave = () => {
    setError(null);
    setSaving(true);
    updateChannelEndpointAsync({ token, id: channel.id, endpoint })
      .then(() => navigate({ pathname: `/channels/detail/${channel.id}` }))
      .catch((e) => {
        setError(e);
      })
      .finally(() => setSaving(false));
  };

  const channelTypeLabels = {
    webhook: "Webhook",
    email: "Email",
    web: "Web",
  };

  React.useEffect(() => {
    setEndpoint(channel.endpoint);
  }, [channel.endpoint]);

  return (
    <React.Fragment>
      {channel.channelType && (
        <CopyableField
          label="Channel Type"
          value={channelTypeLabels[channel.channelType]}
        />
      )}

      {channel.channelType === "webhook" && (
        <InputGroup>
          <TextInput
            label="Webhook Endpoint"
            placeholder="https://..."
            value={endpoint}
            validator={createStartsWithValidator("http")}
            onChange={(e) => setEndpoint(e.target.value)}
          />
        </InputGroup>
      )}
      <AsyncButton
        className="mt-3 btn btn-outline-primary"
        loading={saving}
        disabled={endpoint === channel.endpoint}
        onClick={handleSave}
      >
        Save
      </AsyncButton>
    </React.Fragment>
  );
};
