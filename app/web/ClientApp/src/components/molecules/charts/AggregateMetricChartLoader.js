import React, { Suspense } from "react";
import { Spinner } from "..";
import { useAggregateMetrics } from "../../../api-hooks/metricsApi";

import "./AggregateMetricChart.css";

const NumericChart = React.lazy(() => import("./AggregateMetricChartNumeric"));
const CategoryChart = React.lazy(() => import("./AggregateMetricChartString"));

export const AggregateMetricChartLoader = ({ metric }) => {
  const { id, valueType } = metric;
  const data = useAggregateMetrics({ id, valueType });
  let display = undefined;

  if (metric.valueType === "numeric") {
    display = <NumericChart metric={metric} data={data} />;
  } else if (metric.valueType === "categorical") {
    display = <CategoryChart metric={metric} data={data} />;
  } else {
    display = (
      <div className="text-muted text-center">
        Please set the metric value type.
      </div>
    );
  }

  return (
    <React.Fragment>
      <Suspense fallback={<Spinner />}>{display}</Suspense>
    </React.Fragment>
  );
};
