import React, { Suspense } from "react";
import { Spinner } from "../Spinner";
import { useMetricBin } from "../../../api-hooks/metricsApi";

import { MetricHistogram } from "./MetricHistogram";

import "./AggregateMetricChart.css";

export const MetricHistogramLoader = ({ metric }) => {
  const { id, valueType } = metric;
  const data = useMetricBin({ id, valueType });

  if (!metric.valueType) {
    return (
      <div className="text-muted text-center">
        Please set the metric value type.
      </div>
    );
  }

  return (
    <React.Fragment>
      <Suspense fallback={<Spinner />}>
        {metric.valueType === "numeric" ? (
          <MetricHistogram
            metric={metric}
            data={data}
            xAxis={{ dataKey: "binRange" }}
            yAxis={{ name: "Customer Count", dataKey: "customerCount" }}
          />
        ) : null}

        {metric.valueType === "categorical" ? (
          <MetricHistogram
            metric={metric}
            data={data}
            xAxis={{ dataKey: "stringValue" }}
            yAxis={{ name: "Customer Count", dataKey: "customerCount" }}
          />
        ) : null}
      </Suspense>
    </React.Fragment>
  );
};
