import React from "react";
import { useHistory } from "react-router-dom";
import { useAnalytics } from "../../analytics/analyticsHooks";
import { useAccessToken } from "../../api-hooks/token";
import { createMetricAsync } from "../../api/metricsApi";
import { AsyncButton, BackButton, ErrorCard, Title } from "../molecules";
import {
  InputGroup,
  TextInput,
  createLengthValidator,
  createServerErrorValidator,
} from "../molecules/TextInput";
import Select from "../molecules/selectors/Select";

const valueTypeOptons = [
  { value: "numeric", label: "Numeric" },
  { value: "categorical", label: "Categorical" },
];

const CreateMetric = () => {
  const token = useAccessToken();
  const history = useHistory();
  const { analytics } = useAnalytics();
  const [error, setError] = React.useState();
  const [metric, setMetric] = React.useState({
    commonId: "",
    name: "",
    valueType: "",
  });
  const [creating, setCreating] = React.useState(false);

  const handleCreate = () => {
    setError(null);
    setCreating(true);
    createMetricAsync({
      metric,
      token,
    })
      .then((r) => {
        analytics.track("site:metric_create_success");
        history.push(`/metrics/detail/${r.id}`);
      })
      .catch((e) => {
        analytics.track("site:metric_create_failure");
        setError(e);
      })
      .finally(() => setCreating(false));
  };
  const setSelectedValueType = (o) => {
    setMetric({ ...metric, valueType: o.value });
  };
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/metrics/">
        All Metrics
      </BackButton>
      <Title>Create Metric</Title>
      <hr />

      {error && <ErrorCard error={error} />}

      <div>
        <InputGroup className="mb-1">
          <TextInput
            placeholder="Something human readable"
            value={metric.name}
            label="Friendly Name"
            validator={createLengthValidator(5)}
            onChange={(e) =>
              setMetric({
                ...metric,
                name: e.target.value,
              })
            }
          />
        </InputGroup>
        <InputGroup className="mb-1">
          <TextInput
            placeholder="Something unique"
            value={metric.commonId}
            label="Common Id"
            validator={createServerErrorValidator("CommonId", error)}
            onChange={(e) =>
              setMetric({
                ...metric,
                commonId: e.target.value,
              })
            }
          />
        </InputGroup>

        <Select
          className="mb-1"
          placeholder="Select a metric value type"
          onChange={setSelectedValueType}
          options={valueTypeOptons}
        />

        <AsyncButton
          loading={creating}
          className="btn btn-primary btn-block"
          onClick={handleCreate}
        >
          Create
        </AsyncButton>
      </div>
    </React.Fragment>
  );
};

export default CreateMetric;
