import React from "react";
import {
  Title,
  Subtitle,
  BackButton,
  Spinner,
  ErrorCard,
} from "../../molecules";
import { ToggleSwitch } from "../../molecules/ToggleSwitch";

export const SettingsUtil = ({
  recommender,
  basePath,
  updateErrorHandling,
}) => {
  const [errorHandling, setErrorHandling] = React.useState();

  React.useEffect(() => {
    if (!recommender.loading && !recommender.error) {
      setErrorHandling(
        recommender?.errorHandling || {
          throwOnBadInput: false,
        }
      );
    }
  }, [recommender]);

  React.useEffect(() => {
    if (errorHandling && errorHandling !== recommender.errorHandling) {
      if (
        recommender.errorHandling?.throwOnBadInput !==
        errorHandling?.throwOnBadInput
      ) {
        updateErrorHandling(errorHandling);
      }
    }
  }, [errorHandling]);

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

      {errorHandling && (
        <div className="row">
          <div className="col">
            <h5>Throw on Bad Inputs</h5>
            Setting this to true will cause inputs on the wrong type to throw an
            error, rather than attempt to be silently handled. This is useful
            when testing.
          </div>
          <div className="col-3">
            <ToggleSwitch
              name="Throw on Bad Input"
              id="throw-on-bad-input"
              checked={errorHandling.throwOnBadInput}
              onChange={(v) =>
                setErrorHandling({ ...errorHandling, throwOnBadInput: v })
              }
            />
          </div>
        </div>
      )}
    </React.Fragment>
  );
};
