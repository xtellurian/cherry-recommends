import React from "react";
import { useHubspotCrmCardBehaviour } from "../../../../api-hooks/hubspotApi";
import { setHubspotCrmCardBehaviourAsync } from "../../../../api/hubspotApi";
import { useFeatures } from "../../../../api-hooks/featuresApi";
import {
  BackButton,
  Title,
  Subtitle,
  Selector,
  AsyncButton,
  ErrorCard,
} from "../../../molecules";
import { ToggleSwitch } from "../../../molecules/ToggleSwitch";
import { SettingRow } from "../../../molecules/SettingRow";
import { useAccessToken } from "../../../../api-hooks/token";
import { useParameterSetRecommender } from "../../../../api-hooks/parameterSetRecommendersApi";
import { AsyncSelectParameterSetRecommender } from "../../../molecules/AsyncSelectParameterSetRecommender";

const Top = ({ integratedSystem }) => {
  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to={`/settings/integrations/detail/${integratedSystem.id}`}
      >
        Overview
      </BackButton>
      <Title> Hubspot CRM Card Behaviour</Title>
      <Subtitle>
        {integratedSystem.name || integratedSystem.commonId || "..."}
      </Subtitle>
      <hr />
    </React.Fragment>
  );
};

export const CrmCardBehaviour = ({ integratedSystem }) => {
  const token = useAccessToken();
  const [updateTrigger, setUpdateTrigger] = React.useState({});
  const behaviour = useHubspotCrmCardBehaviour({
    id: integratedSystem.id,
    trigger: updateTrigger,
  });

  const parameterSetRecommender = useParameterSetRecommender({
    id: behaviour.parameterSetRecommenderId,
  });

  const [newRecommender, setRecommender] = React.useState();
  const [featureOptions, setFeatureOptions] = React.useState([]);
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);
  const [include, setInclude] = React.useState(true);

  const features = useFeatures();

  React.useEffect(() => {
    if (features.items) {
      setFeatureOptions(
        features.items.map((f) => ({ label: f.name, value: f }))
      );
    }
  }, [features]);

  React.useEffect(() => {
    if (behaviour.items) {
      setFeatureOptions(
        features.items.map((f) => ({ label: f.name, value: f }))
      );
    }
  }, [behaviour, features]);

  const [selectedFeatures, setSelectedFeatures] = React.useState([]);

  React.useEffect(() => {
    if (behaviour.includedFeatures && behaviour.includedFeatures.length) {
      setSelectedFeatures(behaviour.includedFeatures || []);
    } else if (
      behaviour.excludedFeatures &&
      behaviour.excludedFeatures.length
    ) {
      setSelectedFeatures(behaviour.excludedFeatures || []);
    } else {
      setSelectedFeatures([]);
    }
  }, [behaviour]);

  const [excludedFeatureOptions, setExcludedFeatureOptions] = React.useState(
    []
  );

  React.useEffect(() => {
    if (behaviour && behaviour.includedFeatures) {
      setInclude(true);
      setExcludedFeatureOptions(
        featureOptions.filter((_) =>
          behaviour.includedFeatures.includes(_.value.commonId)
        )
      );
    } else if (behaviour && behaviour.excludedFeatures) {
      setInclude(false);
      setExcludedFeatureOptions(
        featureOptions.filter((_) =>
          behaviour.excludedFeatures.includes(_.value.commonId)
        )
      );
    }
  }, [behaviour, featureOptions]);

  const handleSave = () => {
    setLoading(true);
    const payload = {};
    if (include) {
      payload.includedFeatures = selectedFeatures;
    } else {
      payload.excludedFeatures = selectedFeatures;
    }
    if (
      newRecommender &&
      newRecommender.id !== behaviour.parameterSetRecommenderId
    ) {
      payload.parameterSetRecommenderId = newRecommender.id;
    }
    setHubspotCrmCardBehaviourAsync({
      id: integratedSystem.id,
      token,
      behaviour: payload,
    })
      .then(setUpdateTrigger)
      .catch(setError)
      .finally(() => setLoading(false));
  };
  return (
    <React.Fragment>
      <Top integratedSystem={integratedSystem} />
      {error && <ErrorCard error={error} />}
      <SettingRow
        label="Recommender"
        description="Would you like to show recommendations in HubSpot? Currently, only ParameterSet Recommendations are supported."
      >
        <AsyncSelectParameterSetRecommender
          allowNone={true}
          placeholder={parameterSetRecommender.name || "Choose a recommender"}
          onChange={(v) => setRecommender(v.value)}
        />
      </SettingRow>

      <SettingRow
        label="Include or Exclude Features"
        description="Would you like to include or exclude the features selected below?"
      >
        <ToggleSwitch
          id="include-exclude-toggle"
          onChange={setInclude}
          checked={include}
          name="Include Features"
        />
        <div className="mt-3">
          The Features selected below will be{" "}
          <strong>{include ? "included " : "excluded "}</strong>
          from your Hubspot CRM Card.
        </div>
      </SettingRow>

      <div className="mb-2">
        <h6>{include ? "Included" : "Excluded"} Features</h6>
        <Selector
          isMulti
          isSearchable
          placeholder="Select features to exclude from CRM Cards"
          noOptionsMessage={({ inputValue }) =>
            `No Feature matches ${inputValue}`
          }
          defaultValue={excludedFeatureOptions}
          value={excludedFeatureOptions}
          onChange={(so) => {
            setExcludedFeatureOptions(so);
            setSelectedFeatures([...so.map((_) => _.value.commonId)]);
          }}
          options={featureOptions}
        />
      </div>
      <div className="mt-3">
        <AsyncButton
          loading={loading}
          className="btn btn-primary btn-block"
          onClick={handleSave}
        >
          Save
        </AsyncButton>
      </div>
    </React.Fragment>
  );
};
