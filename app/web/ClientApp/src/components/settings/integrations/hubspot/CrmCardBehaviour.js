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
import { SettingRow } from "../../../molecules/layout/SettingRow";
import { useAccessToken } from "../../../../api-hooks/token";
import { useParameterSetRecommender } from "../../../../api-hooks/parameterSetRecommendersApi";
import { AsyncSelectParameterSetRecommender } from "../../../molecules/selectors/AsyncSelectParameterSetRecommender";
import { AsyncSelectItemsRecommender } from "../../../molecules/selectors/AsyncSelectItemsRecommender";
import { useItemsRecommender } from "../../../../api-hooks/itemsRecommendersApi";

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

const recommenderTypeOptions = [
  { label: "Item Recommender", value: "ITEMS" },
  { label: "Parameter Set Recommender", value: "PARAMETER-SET" },
  { label: "None", value: "None" },
];

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
  const itemsRecommender = useItemsRecommender({
    id: behaviour.itemsRecommenderId,
  });

  const [recommenderType, setRecommenderType] = React.useState();
  // update the recommender type based in which one is not null
  React.useEffect(() => {
    if (behaviour.parameterSetRecommenderId) {
      setRecommenderType(
        recommenderTypeOptions.find((x) => x.value === "PARAMETER-SET")
      );
    } else if (behaviour.itemsRecommenderId) {
      setRecommenderType(
        recommenderTypeOptions.find((x) => x.value === "ITEMS")
      );
    }
  }, [behaviour]);

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
      recommenderType.value === "PARAMETER-SET" &&
      newRecommender.id !== behaviour.parameterSetRecommenderId
    ) {
      payload.parameterSetRecommenderId = newRecommender.id;
    }
    if (
      newRecommender &&
      recommenderType.value === "ITEMS" &&
      newRecommender.id !== behaviour.itemsRecommenderId
    ) {
      payload.itemsRecommenderId = newRecommender.id;
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
        <Selector
          placeholder="Recommender Type"
          value={recommenderType}
          defaultValue={recommenderType}
          onChange={setRecommenderType}
          options={recommenderTypeOptions}
        />
        {!recommenderType ||
          (recommenderType.value === "PARAMETER-SET" && (
            <AsyncSelectParameterSetRecommender
              allowNone={true}
              placeholder={
                parameterSetRecommender.name ||
                "Choose a parameter-set recommender"
              }
              onChange={(v) => setRecommender(v.value)}
            />
          ))}
        {recommenderType && recommenderType.value === "ITEMS" && (
          <AsyncSelectItemsRecommender
            allowNone={true}
            placeholder={itemsRecommender.name || "Choose an items recommender"}
            onChange={(v) => setRecommender(v.value)}
          />
        )}
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
