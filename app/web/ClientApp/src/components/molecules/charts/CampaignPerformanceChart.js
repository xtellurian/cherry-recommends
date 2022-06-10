import React, { Suspense } from "react";
import { EmptyState } from "../empty/EmptyState";
import { Spinner } from "../Spinner";

const Chart = React.lazy(() => import("./TimelineChart"));

const CampaignPerformanceChart = ({ reportData }) => {
  const chartData = reportData?.data
    ? reportData.data.map((v, i) => {
        return {
          timestamp: v.endDate,
          "Additional Revenue": v.additionalRevenue?.toFixed(2),
        };
      })
    : null;

  if (chartData) {
    const minRevenue = Math.floor(
      Math.min(...chartData.map((_) => _["Additional Revenue"]))
    );
    const maxRevenue = Math.ceil(
      Math.max(...chartData.map((_) => _["Additional Revenue"]))
    );
    return (
      <React.Fragment>
        {reportData.loading ? (
          <Spinner />
        ) : (
          <Suspense fallback={<Spinner />}>
            <Chart
              containerHeight={300}
              data={chartData}
              yAxisLabel="Additional Revenue ($)"
              xAxisLabel="Week ending on"
              yAxisDomain={[minRevenue, maxRevenue]}
            />
          </Suspense>
        )}
      </React.Fragment>
    );
  } else {
    return <EmptyState>No Data to Chart</EmptyState>;
  }
};

export default CampaignPerformanceChart;
