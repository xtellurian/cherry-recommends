import React from "react";
import { Spinner, ErrorCard, Typography } from "../../molecules";
import { TimespanSelector } from "../../molecules/selectors/TimespanSelector";
import { ToggleSwitch } from "../../molecules/ToggleSwitch";
import { ScheduleUtil } from "./scheduleUtil";

const initSettings = {
  _isInitial: true,
  throwOnBadInput: false,
  requireConsumptionEvent: false,
  recommendationCacheTime: null,
  expiryDate: null,
};

const anyAreDifferent = (newSettings, oldSettings) => {
  return (
    newSettings?.throwOnBadInput !== oldSettings?.throwOnBadInput ||
    newSettings?.requireConsumptionEvent !==
      oldSettings?.requireConsumptionEvent ||
    newSettings?.recommendationCacheTime !==
      oldSettings?.recommendationCacheTime ||
    newSettings?.expiryDate !== oldSettings?.expiryDate
  );
};

export const SettingRow = ({
  label,
  description,
  children,
  noBorderBottom,
}) => {
  return (
    <div className={`${noBorderBottom ? "" : "border-bottom pb-4"}`}>
      <div className="row mt-4">
        <div className="col">
          <Typography className="semi-bold">{label}</Typography>
          <Typography className="text-secondary mt-1">{description}</Typography>
        </div>
        <div className="col-lg-6 col-md-8 text-center">{children}</div>
      </div>
    </div>
  );
};

export const SettingsUtil = ({ recommender, basePath, updateSettings }) => {
  // const [errorHandling, setErrorHandling] = React.useState();
  const [settings, setSettings] = React.useState(initSettings);
  const [expDate, setExpiryDate] = React.useState(new Date());
  const [expDateEnabled, setExpiryDateEnabled] = React.useState(false);

  if (updateSettings === undefined) {
    throw new Error("updateSettings is a required prop");
  }

  const handleScheduleSettingsChanged = () => {
    const inputExpiry = expDateEnabled ? expDate : null;
    setSettings({
      ...settings,
      expiryDate: inputExpiry,
    });
  };

  React.useEffect(() => {
    handleScheduleSettingsChanged();
  }, [expDate]);

  React.useEffect(() => {
    handleScheduleSettingsChanged();
  }, [expDateEnabled]);

  React.useEffect(() => {
    if (!recommender.loading && !recommender.error) {
      setSettings(
        recommender?.settings || { ...initSettings, _isInitial: false }
      );

      setExpiryDateEnabled(recommender?.settings?.expiryDate !== null);
      setExpiryDate(
        recommender?.settings?.expiryDate
          ? new Date(recommender?.settings?.expiryDate)
          : new Date()
      );
    }
  }, [recommender]);

  React.useEffect(() => {
    if (
      settings &&
      settings !== initSettings &&
      settings !== recommender.settings
    ) {
      if (anyAreDifferent(recommender.settings, settings)) {
        updateSettings(settings);
      }
    }
  }, [settings]);

  return (
    <React.Fragment>
      {recommender.loading && <Spinner />}
      {recommender.error && <ErrorCard error={recommender.error} />}

      {!settings._isInitial && (
        <React.Fragment>
          <SettingRow
            label="Error on Bad Invokation"
            description=" Setting this to true will cause inputs on the wrong type to throw
              an error, rather than attempt to be silently handled. This is
              useful when testing."
          >
            <ToggleSwitch
              name="Throw on Bad Input"
              id="throw-on-bad-input"
              checked={settings.throwOnBadInput}
              onChange={(v) => setSettings({ ...settings, throwOnBadInput: v })}
            />
          </SettingRow>

          <SettingRow
            label="Require Presented Event"
            description="By default, all recommendations are assumed to be consumed, i.e.
              affect the Customer's behaviour in some way. In some situations,
              you may wish to invoke a recommender and only later decide whether
              to consume the recommendation. In that case, you should set this
              to true, and send an event when and if the Customer is affected by
              the recommendation."
          >
            <ToggleSwitch
              name="Require Presented Event"
              id="require-consumption-event"
              checked={settings.requireConsumptionEvent}
              onChange={(v) =>
                setSettings({ ...settings, requireConsumptionEvent: v })
              }
            />
          </SettingRow>

          <SettingRow
            label="Recommendation Cache Time"
            description="By default, a new recommendation will be generated every time this recommender is invoked.
            Setting this value causes Cherry to cache a recommendation for this time period."
          >
            <TimespanSelector
              initialValue={settings.recommendationCacheTime}
              allowNull={true}
              onChange={(v) =>
                setSettings({ ...settings, recommendationCacheTime: v })
              }
            />
          </SettingRow>
          <SettingRow
            label="Schedule"
            description="Choose a schedule for which the recommender will stop producing recommendations.
            Setting an expiry date will make the recommender expire at end of day in UTC timezone."
          >
            <ScheduleUtil
              onOptionChanged={setExpiryDateEnabled}
              onDateChanged={setExpiryDate}
              expiryDate={expDate}
              expiryDateEnabled={expDateEnabled}
            ></ScheduleUtil>
          </SettingRow>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};
