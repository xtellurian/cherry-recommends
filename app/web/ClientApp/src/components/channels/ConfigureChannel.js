import React, { useState, useEffect } from "react";

import { useAccessToken } from "../../api-hooks/token";
import { updateChannelEndpointAsync } from "../../api/channelsApi";
import { useNavigation } from "../../utility/useNavigation";
import { TextInput, createStartsWithValidator } from "../molecules/TextInput";
import { AsyncButton, ErrorCard } from "../molecules";
import { EmailConfiguration } from "./EmailConfiguration";
import { WebChannelConfiguration } from "./WebChannelConfiguration";

const WebhookConfiguration = ({ channel }) => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const [error, setError] = useState();
  const [saving, setSaving] = useState(false);
  const [endpoint, setEndpoint] = useState("");

  const handleSave = () => {
    setError(null);
    setSaving(true);

    updateChannelEndpointAsync({
      token,
      id: channel.id,
      endpoint,
    })
      .then(() => navigate(`/integrations/channels/detail/${channel.id}`))
      .catch((e) => setError(e))
      .finally(() => setSaving(false));
  };

  useEffect(() => {
    if (channel.loading) {
      return;
    }

    setEndpoint(channel.endpoint);
  }, [channel]);

  return (
    <React.Fragment>
      {error ? <ErrorCard error={error} /> : null}
      <TextInput
        label="Webhook Endpoint"
        placeholder="https://..."
        value={endpoint}
        validator={createStartsWithValidator("http")}
        onChange={(e) => setEndpoint(e.target.value)}
      />

      <AsyncButton
        className="float-right mt-3 btn btn-primary"
        loading={saving}
        disabled={endpoint === channel.endpoint}
        onClick={handleSave}
      >
        Save
      </AsyncButton>
    </React.Fragment>
  );
};

export const ConfigureChannel = ({ channel }) => {
  if (channel.channelType === "webhook") {
    return <WebhookConfiguration channel={channel} />;
  }

  if (channel.channelType === "web") {
    return <WebChannelConfiguration channel={channel} />;
  }

  if (channel.channelType === "email") {
    return <EmailConfiguration channel={channel} />;
  }

  return null;
};
