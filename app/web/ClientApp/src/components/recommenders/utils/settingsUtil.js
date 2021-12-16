import React from "react";
import {
  Title,
  Subtitle,
  BackButton,
  Spinner,
  ErrorCard,
} from "../../molecules";
import { SettingRow } from "../../molecules/layout/SettingRow";
import { TimespanSelector } from "../../molecules/selectors/TimespanSelector";
import { ToggleSwitch } from "../../molecules/ToggleSwitch";

const initSettings = {
  _isInitial: true,
  throwOnBadInput: false,
  requireConsumptionEvent: false,
  recommendationCacheTime: null,
};

const anyAreDifferent = (newSettings, oldSettings) => {
  return (
    newSettings?.throwOnBadInput !== oldSettings?.throwOnBadInput ||
    newSettings?.requireConsumptionEvent !==
      oldSettings?.requireConsumptionEvent ||
    newSettings?.recommendationCacheTime !==
      oldSettings?.recommendationCacheTime
  );
};
export const SettingsUtil = ({ recommender, basePath, updateSettings }) => {
  // const [errorHandling, setErrorHandling] = React.useState();
  const [settings, setSettings] = React.useState(initSettings);

  if (updateSettings === undefined) {
    throw new Error("updateSettings is a required prop");
  }

  React.useEffect(() => {
    if (!recommender.loading && !recommender.error) {
      setSettings(
        recommender?.settings || { ...initSettings, _isInitial: false }
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
            label="Require Consumption Event"
            description="By default, all recommendations are assumed to be consumed, i.e.
              affect the Customer's behaviour in some way. In some situations,
              you may wish to invoke a recommender and only later decide whether
              to consume the recommendation. In that case, you should set this
              to true, and send an event when and if the Customer is affected by
              the recommendation."
          >
            <ToggleSwitch
              name="Require Consumption Event"
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
            Setting this value causes Four2 to cache a recommendation for this time period."
          >
            <TimespanSelector
              initialValue={settings.recommendationCacheTime}
              allowNull={true}
              onChange={(v) =>
                setSettings({ ...settings, recommendationCacheTime: v })
              }
            />
          </SettingRow>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};
