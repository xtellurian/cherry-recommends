import React, { Suspense } from "react";

import { useMetricBin } from "../../../../api-hooks/metricsApi";

import { Spinner } from "../../Spinner";
import {
  MetricQuartilesChart,
  MetricQuartilesThumbnailChart,
} from "../MetricQuartilesChart";

export const MetricQuartilesThumbnailChartLoader = ({ metric }) => {
  const { id, valueType } = metric;
  const data = useMetricBin({ id, valueType, binCount: 4 });

  if (!metric.valueType) {
    return null;
  }

  return (
    <React.Fragment>
      <Suspense fallback={<Spinner />}>
        <MetricQuartilesThumbnailChart metric={metric} data={data} />
      </Suspense>
    </React.Fragment>
  );
};

export const MetricQuartilesChartLoader = ({ metric }) => {
  const { id, valueType } = metric;
  const data = useMetricBin({ id, valueType, binCount: 4 });

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
        <MetricQuartilesChart metric={metric} data={data} />
      </Suspense>
    </React.Fragment>
  );
};
