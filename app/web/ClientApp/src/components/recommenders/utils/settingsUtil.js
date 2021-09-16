import React from "react";
import {
  Title,
  Subtitle,
  BackButton,
  Spinner,
  ErrorCard,
} from "../../molecules";
import { ToggleSwitch } from "../../molecules/ToggleSwitch";

export const SettingsUtil = ({ recommender, basePath, updateSettings }) => {
  // const [errorHandling, setErrorHandling] = React.useState();
  const [settings, setSettings] = React.useState();

  if (updateSettings === undefined) {
    throw new Error("updateSettings is a required prop");
  }

  React.useEffect(() => {
    if (!recommender.loading && !recommender.error) {
      setSettings(
        recommender?.settings || {
          throwOnBadInput: false,
          requireConsumptionEvent: false,
        }
      );
    }
  }, [recommender]);

  React.useEffect(() => {
    if (settings && settings !== recommender.settings) {
      if (
        recommender.settings?.throwOnBadInput !== settings?.throwOnBadInput ||
        recommender.settings?.requireConsumptionEvent !==
          settings?.requireConsumptionEvent
      ) {
        updateSettings(settings);
      }
    }
  }, [settings]);

  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to={`${basePath}/detail/${recommender.id}`}
      >
        Back to Recommender
      </BackButton>
      <Title>Settings</Title>
      <Subtitle>{recommender.name}</Subtitle>
      <hr />

      {recommender.loading && <Spinner />}
      {recommender.error && <ErrorCard error={recommender.error} />}

      {settings && (
        <React.Fragment>
          <div className="row">
            <div className="col">
              <h5>Throw on Bad Inputs</h5>
              Setting this to true will cause inputs on the wrong type to throw
              an error, rather than attempt to be silently handled. This is
              useful when testing.
            </div>
            <div className="col-3">
              <ToggleSwitch
                name="Throw on Bad Input"
                id="throw-on-bad-input"
                checked={settings.throwOnBadInput}
                onChange={(v) =>
                  setSettings({ ...settings, throwOnBadInput: v })
                }
              />
            </div>
          </div>

          <div className="row">
            <div className="col">
              <h5>Require Consumption Event</h5>
              By default, all recommendations are assumed to be consumed, i.e.
              affect the tracked user's behaviour in some way. In some
              situations, you may wish to invoke a recommender and only later
              decide whether to consume the recommendation. In that case, you
              should set this to true, and send an event when and if the tracked
              user is affected by the recommendation.
            </div>
            <div className="col-3">
              <ToggleSwitch
                name="Require Consumption Event"
                id="require-consumption-event"
                checked={settings.requireConsumptionEvent}
                onChange={(v) =>
                  setSettings({ ...settings, requireConsumptionEvent: v })
                }
              />
            </div>
          </div>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};
