import React, { Suspense } from "react";
import { Spinner } from "..";
const Chart = React.lazy(() => import("./TargetFeatureChart"));

export const TargetFeatureChartLoader = ({ label, values }) => {
  return (
    <React.Fragment>
      <Suspense fallback={<Spinner />}>
        <Chart label={label} values={values} />
      </Suspense>
    </React.Fragment>
  );
};
