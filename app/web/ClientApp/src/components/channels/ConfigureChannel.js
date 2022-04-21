import React, { useState, useEffect, useMemo } from "react";

import { useAccessToken } from "../../api-hooks/token";
import {
  updateChannelPropertiesAsync,
  updateChannelEndpointAsync,
} from "../../api/channelsApi";
import { useNavigation } from "../../utility/useNavigation";
import {
  InputGroup,
  TextInput,
  createStartsWithValidator,
  numericValidator,
} from "../molecules/TextInput";
import { AsyncButton, ErrorCard, Selector, Typography } from "../molecules";
import { ToggleSwitch } from "../molecules/ToggleSwitch";

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
      .then(() => navigate({ pathname: `/channels/detail/${channel.id}` }))
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
      <InputGroup>
        <TextInput
          label="Webhook Endpoint"
          placeholder="https://..."
          value={endpoint}
          validator={createStartsWithValidator("http")}
          onChange={(e) => setEndpoint(e.target.value)}
        />
      </InputGroup>

      <AsyncButton
        className="float-right mt-3 btn btn-outline-primary"
        loading={saving}
        disabled={endpoint === channel.endpoint}
        onClick={handleSave}
      >
        Save
      </AsyncButton>
    </React.Fragment>
  );
};

const WebConfiguration = ({ channel }) => {
  const token = useAccessToken();
  const { navigate, ensureAbsolutePathsHaveTenantNamePrefixed } =
    useNavigation();
  const [error, setError] = useState();
  const [saving, setSaving] = useState(false);

  const [popupAskForEmail, setPopupAskForEmail] = useState(false);
  const [popupHeader, setPopupHeader] = useState("");
  const [popupSubheader, setPopupSubheader] = useState("");
  const [popupDelay, setPopupDelay] = useState("");
  const [selectedRecommenderId, setSelectedRecommenderId] = useState("");

  const handleSave = () => {
    setError(null);
    setSaving(true);

    updateChannelPropertiesAsync({
      token,
      id: channel.id,
      properties: {
        recommenderId: selectedRecommenderId || null,
        popupAskForEmail,
        popupDelay,
        popupHeader,
        popupSubheader,
      },
    })
      .then(() => navigate({ pathname: `/channels/detail/${channel.id}` }))
      .catch((e) => setError(e))
      .finally(() => setSaving(false));
  };

  const handleSelectRecommender = ({ value }) => {
    setSelectedRecommenderId(value);
  };

  const recommenderOptions = useMemo(
    () =>
      channel?.recommenders?.map((recommender) => ({
        label: recommender.name,
        value: recommender.id,
      })) || [],
    [channel]
  );

  const selectedRecommenderValue = useMemo(
    () =>
      recommenderOptions.find(
        (option) => option.value === selectedRecommenderId
      ),
    [recommenderOptions, selectedRecommenderId]
  );

  const baseUrl = `${
    window.location.origin
  }${ensureAbsolutePathsHaveTenantNamePrefixed("/")}`;

  const jsScriptSnippet = `
    <!-- Start of Cherry Embed Code -->
      <script
        type="text/javascript"
        id="cherry-channel"
        async
        src="https://jschannelscript.blob.core.windows.net/js-channel-script/channel.browser.js?apiKey=<YOUR_API_KEY>&channelId=${channel.id}&baseUrl=${baseUrl}"
      ></script>
    <!-- End of Cherry Embed Code -->
  `;

  const onCopyToClipboard = (value) => {
    navigator.clipboard.writeText(value);
  };

  useEffect(() => {
    if (channel.loading) {
      return;
    }

    setPopupAskForEmail(channel.popupAskForEmail || false);
    setPopupDelay(channel.popupDelay || 100);
    setPopupHeader(channel.popupHeader || "");
    setPopupSubheader(channel.popupSubheader || "");
    setSelectedRecommenderId(channel.recommenderIdToInvoke || "");
  }, [channel]);

  return (
    <React.Fragment>
      {error ? <ErrorCard error={error} /> : null}

      <div className="mt-4">
        <Typography variant="h2" className="font-weight-bold">
          Tracking Code
        </Typography>
        <hr />
      </div>

      <div className="mt-3">
        <pre
          className="rounded-sm"
          style={{ backgroundColor: "var(--cherry-black-alpha)" }}
        >
          <button
            className="btn btn-dark rounded-0 btn-sm float-right"
            onClick={() => onCopyToClipboard(jsScriptSnippet)}
          >
            Copy
          </button>
          <code>{jsScriptSnippet}</code>
        </pre>
      </div>

      <div className="mt-4">
        <Typography variant="h2" className="font-weight-bold">
          Popup Configuration
        </Typography>
        <hr />
      </div>

      <div className="ml-1">
        <div className="mt-3">
          <Typography variant="h4">Choose a recommender</Typography>
          <Selector
            className="mt-1"
            value={selectedRecommenderValue}
            options={recommenderOptions}
            onChange={handleSelectRecommender}
          />
        </div>

        <div className="mt-3">
          <ToggleSwitch
            name="Ask For Email Popup"
            id="ask-for-email-popup"
            checked={popupAskForEmail}
            onChange={() =>
              setPopupAskForEmail((oldPopupAskForEmail) => !oldPopupAskForEmail)
            }
          />
          <Typography variant="h4" className="ml-1">
            Enable popup
          </Typography>
        </div>

        <InputGroup className="mt-3">
          <TextInput
            label="Popup Delay (in millisecond)"
            value={popupDelay}
            validator={numericValidator(true, 100)}
            onChange={(e) => setPopupDelay(e.target.value)}
          />
        </InputGroup>

        <InputGroup className="mt-3">
          <TextInput
            label="Popup Header"
            value={popupHeader}
            onChange={(e) => setPopupHeader(e.target.value)}
          />
        </InputGroup>

        <InputGroup className="mt-3">
          <TextInput
            label="Popup Subheader"
            value={popupSubheader}
            onChange={(e) => setPopupSubheader(e.target.value)}
          />
        </InputGroup>
      </div>

      <div className="clearfix">
        <AsyncButton
          className="float-right mt-3 btn btn-outline-primary"
          loading={saving}
          disabled={numericValidator(true, 100)(popupDelay).length > 0}
          onClick={handleSave}
        >
          Save
        </AsyncButton>
      </div>
    </React.Fragment>
  );
};

export const ConfigureChannel = ({ channel }) => {
  if (channel.channelType === "webhook") {
    return <WebhookConfiguration channel={channel} />;
  }

  if (channel.channelType === "web") {
    return <WebConfiguration channel={channel} />;
  }

  return null;
};
