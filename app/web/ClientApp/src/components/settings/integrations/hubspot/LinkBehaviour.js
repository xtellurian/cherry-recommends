import React from "react";
import {
  ErrorCard,
  Spinner,
  Title,
  Subtitle,
  BackButton,
} from "../../../molecules";
import { Selector } from "../../../molecules/selectors/Select";
import { ToggleSwitch } from "../../../molecules/ToggleSwitch";
import {
  useHubspotWebhookBehaviour,
  useHubspotClientAllContactProperties,
} from "../../../../api-hooks/hubspotApi";
import { setHubspotWebhookBehaviourAsync } from "../../../../api/hubspotApi";
import { useAccessToken } from "../../../../api-hooks/token";
import { TextInput, InputGroup } from "../../../molecules/TextInput";
import { SettingRow } from "../../../molecules/settings/SettingRow";

const Top = ({ integratedSystem }) => {
  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to={`/settings/integrations/detail/${integratedSystem.id}`}
      >
        Overview
      </BackButton>
      <Title> Hubspot Linking Behaviour</Title>
      <Subtitle>
        {integratedSystem.name || integratedSystem.commonId || "..."}
      </Subtitle>
      <hr />
    </React.Fragment>
  );
};

export const HubspotLinkBehaviour = ({ integratedSystem }) => {
  const [loading, setLoading] = React.useState(false);
  const token = useAccessToken();
  const [updateTrigger, setUpdateTrigger] = React.useState({});
  const [error, setError] = React.useState();
  const behaviour = useHubspotWebhookBehaviour({
    id: integratedSystem.id,
    trigger: updateTrigger,
  });

  const properties = useHubspotClientAllContactProperties({
    id: integratedSystem.id,
  });

  const hubspotPropertiesSelectable =
    !properties.error && !properties.loading
      ? properties.map((u) => ({
          label: u.label || u.name,
          value: u,
        }))
      : [];

  const handleUploadPropertyName = (name) => {
    if (name !== behaviour.commonUserIdPropertyName) {
      setError(null);
      setLoading(true);
      setHubspotWebhookBehaviourAsync({
        token,
        id: integratedSystem.id,
        behaviour: {
          ...behaviour,
          commonUserIdPropertyName: name,
        },
      })
        .then(setUpdateTrigger)
        .catch(setError)
        .finally(() => setLoading(false));
    }
  };

  const [propertyPrefix, setPropertyPrefix] = React.useState(
    behaviour.propertyPrefix || ""
  );

  React.useEffect(() => {
    if (
      behaviour.propertyPrefix &&
      behaviour.propertyPrefix !== propertyPrefix
    ) {
      setPropertyPrefix(behaviour.propertyPrefix);
    }
  }, [behaviour]);

  const handleUpdatePropertyPrefix = (prefix) => {
    if (prefix !== behaviour.propertyPrefix) {
      setLoading(true);
      setHubspotWebhookBehaviourAsync({
        token,
        id: integratedSystem.id,
        behaviour: {
          ...behaviour,
          propertyPrefix: prefix,
        },
      })
        .then(setUpdateTrigger)
        .catch(setError)
        .finally(() => setLoading(false));
    }
  };

  if (behaviour.loading) {
    return (
      <React.Fragment>
        <Top integratedSystem={integratedSystem} />
        <Spinner>Loading Behaviour</Spinner>
      </React.Fragment>
    );
  }
  if (loading) {
    return (
      <React.Fragment>
        <Top integratedSystem={integratedSystem} />
        <Spinner>Loading</Spinner>
        {error && <ErrorCard error={error} />}
      </React.Fragment>
    );
  }
  return (
    <React.Fragment>
      <Top integratedSystem={integratedSystem} />
      <div>
        {error && <ErrorCard error={error} />}
        <SettingRow
          label="Track users automatically"
          description="Should a tracked user be created if they don't exist already?"
        >
          <ToggleSwitch
            name="Create Users Automatically"
            id="create-if-not-exist-toggle"
            onChange={(v) => {
              setError(null);
              setLoading(true);
              setHubspotWebhookBehaviourAsync({
                token,
                id: integratedSystem.id,
                behaviour: { ...behaviour, createUserIfNotExist: v },
              })
                .then(setUpdateTrigger)
                .catch(setError)
                .finally(() => setLoading(false));
            }}
            checked={behaviour.createUserIfNotExist}
          />
        </SettingRow>
        <SettingRow
          label="Common User ID Property Name"
          description="Which Hubspot Property should we use as the Four2 Common User ID? Click the button to use Object ID"
        >
          <button
            className="btn btn-outline-info btn-block"
            onClick={() => handleUploadPropertyName("")}
          >
            Use Hubspot Object ID
          </button>
          or
          <Selector
            isSearchable={true}
            placeholder="Choose another Contact Property"
            defaultValue={{
              label: behaviour.commonUserIdPropertyName || "Hubspot Object ID",
            }}
            // defaultValue={argumentType || argumentTypeOptions[0].value}
            onChange={(v) => handleUploadPropertyName(v.value.name)}
            options={hubspotPropertiesSelectable}
          />
        </SettingRow>
        <SettingRow
          label="Property Prefix"
          description="How should we prefix Hubspot Properties?"
        >
          <InputGroup>
            <TextInput
              onChange={(v) => setPropertyPrefix(v.target.value)}
              onBlur={() => handleUpdatePropertyPrefix(propertyPrefix)}
              value={propertyPrefix}
              placeholder="e.g. 'hubspot_'"
            />
          </InputGroup>
        </SettingRow>
      </div>
    </React.Fragment>
  );
};
