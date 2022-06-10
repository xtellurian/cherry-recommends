import React, { Suspense } from "react";
import { Spinner } from "../Spinner";

const Chart = React.lazy(() => import("./TimelineChart"));

const CampaignAPVChart = ({ reportData }) => {
  const chartData = reportData?.data?.map((v, i) => {
    return {
      timestamp: v.endDate,
      Overall: v.meanGrossRevenue.toFixed(2),
      Control: v.baselineMeanGrossRevenue.toFixed(2),
    };
  });
  return (
    <React.Fragment>
      {reportData.loading ? (
        <Spinner />
      ) : (
        <Suspense fallback={<Spinner />}>
          <Chart
            containerHeight={300}
            data={chartData}
            yAxisLabel="Average basket size ($)"
            xAxisLabel="Week ending on"
            yAxisDomain={[0, "auto"]}
          />
        </Suspense>
      )}
    </React.Fragment>
  );
};

export default CampaignAPVChart;
