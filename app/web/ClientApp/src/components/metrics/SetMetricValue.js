import React from "react";

import { useParams } from "react-router-dom";
import { setCustomerMetricAsync } from "../../api/customersApi";
import { AsyncSelectCustomer } from "../molecules/selectors/AsyncSelectCustomer";
import { CustomerListItem } from "../molecules/CustomerLists";
import { useMetric } from "../../api-hooks/metricsApi";
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

const SetMetricValue = () => {
  const token = useAccessToken();
  const { id } = useParams();
  const metric = useMetric({ id });
  const [error, setError] = React.useState();
  const [trackedUser, setTrackedUser] = React.useState();

  const [metricValue, setMetricValue] = React.useState("");
  const [loading, setLoading] = React.useState(false);
  const [valueWasSet, setValueWasSet] = React.useState();

  React.useEffect(() => {
    if (trackedUser) {
      setValueWasSet(null);
    }
  }, [trackedUser]);

  const handleSetValue = () => {
    setLoading(true);
    setCustomerMetricAsync({
      id: trackedUser.id,
      token,
      metricId: metric.commonId,
      value: metricValue,
    })
      .then(setValueWasSet)
      .catch((e) => setError(e))
      .finally(() => setLoading(false));
  };

  return (
    <React.Fragment>
      <BackButton className="float-right" to={`/metrics/detail/${id}`}>
        Back to Metrics
      </BackButton>
      <Title>Set Metric Value</Title>
      <Subtitle>{metric.name}</Subtitle>
      <hr />
      {metric.loading && <Spinner />}
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
            label={`Metric Value was set (version ${valueWasSet.version})`}
          >
            <CopyableField label={metric.name} value={valueWasSet.value} />
          </NoteBox>
        </div>
      )}

      {trackedUser && !valueWasSet && (
        <React.Fragment>
          <p>Set a new value</p>
          <InputGroup>
            <TextInput
              disabled={loading}
              label={`${metric.name} Value`}
              placeholder={`Metric Value for ${
                trackedUser.name || trackedUser.commonId
              }`}
              value={metricValue}
              onChange={(v) => setMetricValue(v.target.value)}
            />
          </InputGroup>
          <AsyncButton
            disabled={!trackedUser || !metricValue}
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

export default SetMetricValue;
