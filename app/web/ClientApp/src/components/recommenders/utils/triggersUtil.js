import React from "react";
import {
  Title,
  Subtitle,
  BackButton,
  Spinner,
  ErrorCard,
  AsyncButton,
} from "../../molecules";

import { SettingRow } from "../../molecules/layout/SettingRow";
import { AsyncSelectFeature } from "../../molecules/selectors/AsyncSelectFeature";
import { useAccessToken } from "../../../api-hooks/token";
import {
  InputGroup,
  TextInput,
  createRequiredByServerValidator,
} from "../../molecules/TextInput";

export const TriggersUtil = ({
  error,
  recommender,
  triggerCollection,
  setTriggerAsync,
  basePath,
}) => {
  const token = useAccessToken();
  const [saving, setSaving] = React.useState(false);
  const [featuresChangedTriggerName, setFeaturesChangedTriggerName] =
    React.useState(triggerCollection?.featuresChanged?.name || "");
  const [featureOptions, setFeaturesOptions] = React.useState();
  const [featureCommonIds, setFeatureCommonIds] = React.useState();
  React.useEffect(() => {
    if (featureOptions) {
      setFeatureCommonIds(featureOptions.map((o) => o.value.commonId));
    } else {
      setFeatureCommonIds(
        triggerCollection.featuresChanged?.featureCommonIds || []
      );
    }
  }, [featureOptions]);

  const isFeaturesChangesTriggerActive =
    triggerCollection.featuresChanged?.featureCommonIds &&
    triggerCollection.featuresChanged.featureCommonIds.length > 0;

  const handleSave = () => {
    setSaving(true);
    let featuresChanged = {
      name: featuresChangedTriggerName,
      featureCommonIds,
    };
    // check if null and empty, just shoot up null
    if (
      !featuresChanged.name &&
      featuresChanged.featureCommonIds.length === 0
    ) {
      featuresChanged = null;
    }
    setTriggerAsync({
      token,
      id: recommender.id,
      trigger: {
        featuresChanged,
      },
    }).finally(() => setSaving(false));
  };

  return (
    <React.Fragment>
      {error && <ErrorCard error={error} />}
      {triggerCollection.loading && <Spinner />}
      {!triggerCollection.loading && (
        <div>
          <SettingRow
            label="Features Changed Trigger"
            description="Trigger an invokation when these Features change."
          >
            <InputGroup>
              <TextInput
                validator={createRequiredByServerValidator(error)}
                placeholder="Label the trigger."
                label="Trigger Name"
                value={featuresChangedTriggerName}
                onChange={(e) => setFeaturesChangedTriggerName(e.target.value)}
              />
            </InputGroup>
            <AsyncSelectFeature
              isMulti={true}
              placeholder="Select Features"
              onChange={setFeaturesOptions}
              defaultCommonIds={
                triggerCollection?.featuresChanged?.featureCommonIds
              }
            />
            <div
              className={`text-right ${
                isFeaturesChangesTriggerActive && "text-success"
              }`}
            >
              {isFeaturesChangesTriggerActive ? "Active" : "Inactive"}
            </div>
          </SettingRow>
        </div>
      )}

      <div className="mt-2">
        <AsyncButton
          className="btn btn-primary btn-block"
          onClick={handleSave}
          loading={saving}
        >
          Save
        </AsyncButton>
      </div>
    </React.Fragment>
  );
};
