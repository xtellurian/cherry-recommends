import React from "react";
import { useAccessToken } from "../../../../api-hooks/token";
import { createMetricGeneratorAsync } from "../../../../api/metricGeneratorsApi";
import { AsyncButton, ErrorCard } from "../../../molecules";
import SelectMetric from "../../../molecules/selectors/AsyncSelectMetric";

export const CreateJoinMetricsGenerator = ({ metric, onCreated }) => {
  const token = useAccessToken();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);

  const [metric1, setMetric1] = React.useState();
  const [metric2, setMetric2] = React.useState();

  const handleCreate = () => {
    setLoading(true);
    createMetricGeneratorAsync({
      token,
      generator: {
        generatorType: "joinTwoMetrics",
        metricCommonId: metric.commonId,
        joinTwoMetrics: {
          joinType: "divide",
          metric1Id: metric1?.id,
          metric2Id: metric2?.id,
        },
      },
    })
      .then(onCreated)
      .catch(setError)
      .finally(() => setLoading(false));
  };

  React.useEffect(() => {
    // chech compare to self
    let newError = null;
    if (metric1) {
      if (metric1.id === metric.id) {
        newError = { title: "Do not choose self." };
      }
    }
    if (metric2) {
      if (metric2.id === metric.id) {
        newError = { title: "Do not choose self." };
      }
    }

    // compare to each other
    if (metric1 && metric2) {
      // chose both
      if (metric1.id === metric2.id) {
        newError = { title: "You cannot choose the same metric twice." };
      }
    }
    setError(newError);
  }, [metric1, metric2]);
  return (
    <>
      {error && <ErrorCard error={error} />}

      <div>
        <label>Numerator</label>

        <SelectMetric
          scope="global"
          placeholder="Select a numerator"
          onChange={(o) => setMetric1(o.value)}
        />
        <hr />
        <label>Denominator</label>
        <SelectMetric
          scope="global"
          placeholder="Select a denominator"
          onChange={(o) => setMetric2(o.value)}
        />
      </div>
      <div className="mt-2">
        <AsyncButton
          className="btn btn-primary btn-block"
          loading={loading}
          onClick={handleCreate}
        >
          Save
        </AsyncButton>
      </div>
    </>
  );
};
