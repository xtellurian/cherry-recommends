import React from "react";

import { useParams } from "react-router-dom";
import { setCustomerFeatureAsync } from "../../api/customersApi";
import { AsyncSelectCustomer } from "../molecules/selectors/AsyncSelectCustomer";
import { CustomerListItem } from "../molecules/CustomerLists";
import { useFeature } from "../../api-hooks/featuresApi";
import {
  Title,
  Subtitle,
  Spinner,
  BackButton,
  ErrorCard,
  AsyncButton,
} from "../molecules";
import { InputGroup, TextInput } from "../molecules/TextInput";
import { useAccessToken } from "../../api-hooks/token";
import { NoteBox } from "../molecules/NoteBox";
import { CopyableField } from "../molecules/fields/CopyableField";

export const SetFeatureValue = () => {
  const token = useAccessToken();
  const { id } = useParams();
  const feature = useFeature({ id });
  const [error, setError] = React.useState();
  const [trackedUser, setTrackedUser] = React.useState();

  const [featureValue, setFeatureValue] = React.useState("");
  const [loading, setLoading] = React.useState(false);
  const [valueWasSet, setValueWasSet] = React.useState();

  React.useEffect(() => {
    if (trackedUser) {
      setValueWasSet(null);
    }
  }, [trackedUser]);

  const handleSetValue = () => {
    setLoading(true);
    setCustomerFeatureAsync({
      id: trackedUser.id,
      token,
      featureId: feature.commonId,
      value: featureValue,
    })
      .then(setValueWasSet)
      .catch((e) => setError(e))
      .finally(() => setLoading(false));
  };

  return (
    <React.Fragment>
      <BackButton className="float-right" to={`/features/detail/${id}`}>
        Back to Feature
      </BackButton>
      <Title>Set Feature Value</Title>
      <Subtitle>{feature.name}</Subtitle>
      <hr />
      {feature.loading && <Spinner />}
      {error && <ErrorCard error={error} />}

      <AsyncSelectCustomer
        placeholder="Choose a Customer"
        onChange={(v) => setTrackedUser(v.value)}
      />
      {trackedUser && <CustomerListItem customer={trackedUser} />}
      <hr />

      {valueWasSet && (
        <div className="">
          <NoteBox
            label={`Feature Value was set (version ${valueWasSet.version})`}
          >
            <CopyableField label={feature.name} value={valueWasSet.value} />
          </NoteBox>
        </div>
      )}

      {trackedUser && !valueWasSet && (
        <React.Fragment>
          <p>Set a new value</p>
          <InputGroup>
            <TextInput
              disabled={loading}
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
