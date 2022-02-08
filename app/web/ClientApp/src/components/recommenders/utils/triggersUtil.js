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
import AsyncSelectMetric from "../../molecules/selectors/AsyncSelectMetric";
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
  const [metricsChangedTriggerName, setMetricsChangedTriggerName] =
    React.useState(triggerCollection?.metricsChanged?.name || "");
  const [metricOptions, setMetricOptions] = React.useState();
  const [metricCommonIds, setMetricCommonIds] = React.useState();
  React.useEffect(() => {
    if (metricOptions) {
      setMetricCommonIds(metricOptions.map((o) => o.value.commonId));
    } else {
      setMetricCommonIds(
        triggerCollection.metricsChanged?.metricCommonIds || []
      );
    }
  }, [metricOptions]);

  const isMetricsChangesTriggerActive =
    triggerCollection.metricsChanged?.metricCommonIds &&
    triggerCollection.metricsChanged.metricCommonIds.length > 0;

  const handleSave = () => {
    setSaving(true);
    let metricsChanged = {
      name: metricsChangedTriggerName,
      metricCommonIds,
    };
    // check if null and empty, just shoot up null
    if (!metricsChanged.name && metricsChanged.metricCommonIds.length === 0) {
      metricsChanged = null;
    }
    setTriggerAsync({
      token,
      id: recommender.id,
      trigger: {
        metricsChanged,
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
            label="Metrics Changed Trigger"
            description="Trigger an invokation when these Metrics change value."
          >
            <InputGroup>
              <TextInput
                validator={createRequiredByServerValidator(error)}
                placeholder="Label the trigger."
                label="Trigger Name"
                value={metricsChangedTriggerName}
                onChange={(e) => setMetricsChangedTriggerName(e.target.value)}
              />
            </InputGroup>
            <AsyncSelectMetric
              isMulti={true}
              placeholder="Select Metrics"
              onChange={setMetricOptions}
              defaultCommonIds={
                triggerCollection?.metricsChanged?.metricCommonIds
              }
            />
            <div
              className={`text-right ${
                isMetricsChangesTriggerActive && "text-success"
              }`}
            >
              {isMetricsChangesTriggerActive ? "Active" : "Inactive"}
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
