import React, { Suspense } from "react";
import { Spinner } from "../../molecules";
import { EmptyList } from "../../molecules";
const RevenueChart = React.lazy(() =>
  import("../../molecules/charts/RevenueChart")
);

export const RevenueChartUtil = ({ recommender, rootPath, actions }) => {
  return (
    <React.Fragment>
      {actions && actions.length > 0 && (
        <Suspense fallback={<Spinner />}>
          <RevenueChart actions={actions} showTrackedUserLink={true} />
        </Suspense>
      )}
      {actions && actions.length === 0 && (
        <EmptyList>This recommender has no associated revenue.</EmptyList>
      )}
    </React.Fragment>
  );
};
