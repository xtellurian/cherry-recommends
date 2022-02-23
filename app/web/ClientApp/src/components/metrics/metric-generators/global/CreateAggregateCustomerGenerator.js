import React from "react";
import { useAccessToken } from "../../../../api-hooks/token";
import { createMetricGeneratorAsync } from "../../../../api/metricGeneratorsApi";
import { AsyncButton, ErrorCard, Selector } from "../../../molecules";
import SelectMetric from "../../../molecules/selectors/AsyncSelectMetric";
import { TextInput } from "../../../molecules/TextInput";

const numericAggregateOptions = [
  {
    value: "sum",
    label: "Sum",
  },
  {
    value: "mean",
    label: "Mean",
  },
];
const categoricalAggregateOptions = [
  {
    value: "sum",
    label: "Sum",
  },
];

export const CreateAggregateCustomerGenerator = ({ metric, onCreated }) => {
  const token = useAccessToken();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);
  const [metricToAggregate, setMetricToAggregate] = React.useState();

  const [aggregationOptions, setAggregationOptions] = React.useState([]);

  const [aggregateCustomerMetric, setAggregateCustomerMetric] = React.useState({
    metricId: null,
    aggregationType: "",
    categoricalValue: "",
  });

  React.useEffect(() => {
    if (metricToAggregate) {
      if (metricToAggregate.id === metric.id) {
        setError({ title: "A metric can't aggregate itself" });
      } else {
        if (metricToAggregate.valueType === "numeric") {
          setAggregationOptions(numericAggregateOptions);
        } else if (metricToAggregate.valueType === "categorical") {
          setAggregationOptions(categoricalAggregateOptions);
        }
        setAggregateCustomerMetric({
          ...aggregateCustomerMetric,
          metricId: metricToAggregate.id,
        });
      }
    }
  }, [metricToAggregate]);
  const handleCreate = () => {
    setLoading(true);
    createMetricGeneratorAsync({
      token,
      useInternalId: true,
      generator: {
        metricCommonId: metric.commonId,
        generatorType: "aggregateCustomerMetric",
        aggregateCustomerMetric,
      },
    })
      .then(onCreated)
      .catch(setError)
      .finally(() => setLoading(false));
  };

  const canCreate =
    aggregateCustomerMetric &&
    aggregateCustomerMetric.metricId &&
    aggregateCustomerMetric.aggregationType;
  return (
    <>
      {error && <ErrorCard error={error} />}
      <div>
        <label>Select a customer metric to aggregate</label>
        <SelectMetric
          scope="customer"
          placeholder="Select a Metric to aggregate"
          onChange={(o) => setMetricToAggregate(o.value)}
        />
      </div>

      {aggregationOptions.length > 0 && (
        <div>
          <label>Choose an aggregation</label>
          <Selector
            onChange={(o) =>
              setAggregateCustomerMetric({
                ...aggregateCustomerMetric,
                aggregationType: o.value,
              })
            }
            options={aggregationOptions}
          />
        </div>
      )}

      {metricToAggregate?.valueType === "categorical" &&
        aggregateCustomerMetric.aggregationType && (
          <>
            <label>Choose a category to sum</label>
            <TextInput
              value={aggregateCustomerMetric.categoricalValue}
              onChange={(v) =>
                setAggregateCustomerMetric({
                  ...aggregateCustomerMetric,
                  categoricalValue: v.target.value,
                })
              }
            />
          </>
        )}

      <AsyncButton
        className="mt-3 btn btn-primary btn-block"
        loading={loading}
        disabled={!canCreate}
        onClick={handleCreate}
      >
        Create
      </AsyncButton>
    </>
  );
};
