import React from "react";

import { useAnalytics } from "../../analytics/analyticsHooks";
import { useAccessToken } from "../../api-hooks/token";
import { createMetricAsync } from "../../api/metricsApi";
import {
  AsyncButton,
  ErrorCard,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
} from "../molecules";
import {
  InputGroup,
  TextInput,
  createLengthValidator,
  createServerErrorValidator,
} from "../molecules/TextInput";
import Select from "../molecules/selectors/Select";
import { useCommonId } from "../../utility/utility";

import { hash } from "../menu/MenuIA";
import CreatePageLayout from "../molecules/layout/CreatePageLayout";
import { useNavigation } from "../../utility/useNavigation";

const valueTypeOptions = [
  { value: "numeric", label: "Numeric" },
  { value: "categorical", label: "Categorical" },
];

const scopeOptions = [
  { value: "customer", label: "Customer" },
  { value: "business", label: "Business" },
  { value: "global", label: "Global" },
];

const CreateMetric = () => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const { analytics } = useAnalytics();
  const { generateCommonId } = useCommonId();
  const [error, setError] = React.useState();
  const [metric, setMetric] = React.useState({
    commonId: "",
    name: "",
    valueType: "numeric",
    scope: null,
  });
  const [creating, setCreating] = React.useState(false);
  const isCustomerOrBusiness =
    metric.scope === "customer" || metric.scope === "business";

  const handleCreate = () => {
    setError(null);
    setCreating(true);
    createMetricAsync({
      metric,
      token,
    })
      .then((r) => {
        analytics.track("site:metric_create_success");
        navigate(`/metrics/detail/${r.id}`);
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
  const setSelectedScope = (s) => {
    setMetric({ ...metric, scope: s.value });
  };

  React.useEffect(() => {
    setMetric({
      ...metric,
      commonId: generateCommonId(metric.name),
    });
  }, [metric.name]);

  return (
    <CreatePageLayout
      createButton={
        <AsyncButton
          loading={creating}
          className="btn btn-primary"
          onClick={handleCreate}
        >
          Create
        </AsyncButton>
      }
    >
      <MoveUpHierarchyPrimaryButton
        to={{
          pathname: "/metrics/",
          hash: hash.metrics,
        }}
      >
        Back to Metrics
      </MoveUpHierarchyPrimaryButton>

      <PageHeading title="Create Metric" showHr />

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
          placeholder="Choose a scope"
          onChange={setSelectedScope}
          options={scopeOptions}
        />
        {isCustomerOrBusiness && (
          <Select
            className="mb-1"
            placeholder="Select a metric value type"
            onChange={setSelectedValueType}
            options={valueTypeOptions}
            defaultValue={valueTypeOptions[0]}
          />
        )}
      </div>
    </CreatePageLayout>
  );
};

export default CreateMetric;
