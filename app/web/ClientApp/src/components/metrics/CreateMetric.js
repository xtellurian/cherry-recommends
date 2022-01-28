import React from "react";
import { useHistory } from "react-router-dom";
import { useAccessToken } from "../../api-hooks/token";
import { createMetricAsync } from "../../api/metricsApi";
import { BackButton, ErrorCard, Title } from "../molecules";
import {
  InputGroup,
  TextInput,
  createLengthValidator,
  createServerErrorValidator,
} from "../molecules/TextInput";

const CreateMetric = () => {
  const token = useAccessToken();
  const history = useHistory();
  const [error, setError] = React.useState();
  const [metric, setMetric] = React.useState({
    commonId: "",
    name: "",
  });

  const handleCreate = () => {
    setError(null);
    createMetricAsync({
      metric,
      token,
    })
      .then((r) => history.push(`/metrics/detail/${r.id}`))
      .catch((e) => setError(e));
  };
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/metrics">
        All Metrics
      </BackButton>
      <Title>Create Metric</Title>
      <hr />

      {error && <ErrorCard error={error} />}

      <div>
        <InputGroup>
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
        <InputGroup>
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

        <button className="btn btn-primary" onClick={handleCreate}>
          Create
        </button>
      </div>
    </React.Fragment>
  );
};

export default CreateMetric;