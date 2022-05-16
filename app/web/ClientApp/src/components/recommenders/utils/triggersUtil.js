import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircle } from "@fortawesome/free-solid-svg-icons";

import {
  Spinner,
  ErrorCard,
  AsyncButton,
  EmptyState,
  Typography,
} from "../../molecules";

import AsyncSelectMetric from "../../molecules/selectors/AsyncSelectMetric";
import { useAccessToken } from "../../../api-hooks/token";
import {
  TextInput,
  createRequiredByServerValidator,
} from "../../molecules/TextInput";

const SettingRow = ({ label, description, children }) => {
  return (
    <div className="row mt-4">
      <div className="col">
        <Typography className="semi-bold">{label}</Typography>
        <Typography className="text-secondary mt-1">{description}</Typography>
      </div>
      <div className="col-lg-6 col-md-8 text-center">{children}</div>
    </div>
  );
};

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

  if (recommender.targetType === "business") {
    return (
      <React.Fragment>
        <div>
          <EmptyState>Business Recommenders do not support triggers</EmptyState>
        </div>
      </React.Fragment>
    );
  }

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
            <TextInput
              validator={createRequiredByServerValidator(error)}
              placeholder="Label the trigger"
              label="Trigger Name"
              value={metricsChangedTriggerName}
              onChange={(e) => setMetricsChangedTriggerName(e.target.value)}
            />

            <AsyncSelectMetric
              label="Select Metrics"
              isMulti={true}
              placeholder="Select Metrics"
              onChange={setMetricOptions}
              defaultCommonIds={
                triggerCollection?.metricsChanged?.metricCommonIds
              }
            />
            <div className="d-flex align-items-center justify-content-end">
              <FontAwesomeIcon
                icon={faCircle}
                className={`${
                  isMetricsChangesTriggerActive
                    ? "text-success"
                    : "text-secondary"
                }`}
              />
              <Typography className="ml-2">
                {isMetricsChangesTriggerActive ? "Active" : "Inactive"}
              </Typography>
            </div>
          </SettingRow>
        </div>
      )}

      <div className="mt-4">
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
