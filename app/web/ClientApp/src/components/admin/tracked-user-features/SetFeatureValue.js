import React from "react";

import { useParams } from "react-router-dom";
import { setTrackedUserFeatureAsync } from "../../../api/trackedUsersApi";
import { AsyncSelectTrackedUser } from "../../molecules/selectors/AsyncSelectTrackedUser";
import { TrackedUserListItem } from "../../molecules/TrackedUser";
import { useFeature } from "../../../api-hooks/featuresApi";
import {
  Title,
  Subtitle,
  Spinner,
  BackButton,
  ErrorCard,
  AsyncButton,
} from "../../molecules";
import { InputGroup, TextInput } from "../../molecules/TextInput";
import { useAccessToken } from "../../../api-hooks/token";

export const SetFeatureValue = () => {
  const token = useAccessToken();
  const { id } = useParams();
  const feature = useFeature({ id });
  const [error, setError] = React.useState();
  const [trackedUser, setTrackedUser] = React.useState();

  const [featureValue, setFeatureValue] = React.useState("");
  const [loading, setLoading] = React.useState(false);

  const handleSetValue = () => {
    setLoading(true);
    setTrackedUserFeatureAsync({
      id: trackedUser.id,
      token,
      featureId: feature.commonId,
      value: featureValue,
    })
      .then(() => alert("Set Value"))
      .catch((e) => setError(e))
      .finally(() => setLoading(false));
  };

  return (
    <React.Fragment>
      <BackButton className="float-right" to={`/admin/features/detail/${id}`}>
        Back to Feature
      </BackButton>
      <Title>Set Feature Value</Title>
      <Subtitle>{feature.name}</Subtitle>
      <hr />
      {feature.loading && <Spinner />}
      {error && <ErrorCard error={error} />}

      <AsyncSelectTrackedUser
        placeholder="Choose a tracked user"
        onChange={(v) => setTrackedUser(v.value)}
      />
      {trackedUser && <TrackedUserListItem trackedUser={trackedUser} />}
      <hr />

      {trackedUser && (
        <React.Fragment>
          <p>Set a new value</p>
          <InputGroup>
            <TextInput
              label={`${feature.name} Value`}
              placeholder={`Feature Value for ${
                trackedUser.name || trackedUser.commonId
              }`}
              value={featureValue}
              onChange={(v) => setFeatureValue(v.target.value)}
            />
          </InputGroup>
          <AsyncButton
            disabled={!trackedUser || !featureValue}
            loading={loading}
            onClick={handleSetValue}
            className="btn btn-primary btn-block"
          >
            Save
          </AsyncButton>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};
