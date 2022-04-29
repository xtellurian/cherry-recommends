import React from "react";
import { useMetrics } from "../../api-hooks/metricsApi";
import { Spinner } from "../molecules";
import { CardSection, Label } from "../molecules/layout/CardSection";
import { MetricRow } from "../metrics/MetricRow";
const Metrics = () => {
  var metrics = useMetrics();
  return (
    <React.Fragment>
      <CardSection className="p-4">
        <Label>Metrics</Label>
        {metrics.loading ? (
          <Spinner />
        ) : (
          <div>
            {metrics.items.map((m) => (
              <MetricRow metric={m} key={m.id} />
            ))}
          </div>
        )}
      </CardSection>
    </React.Fragment>
  );
};
export default Metrics;
