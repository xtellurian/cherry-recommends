import React, { Suspense } from "react";
import { Spinner } from "../Spinner";

const Chart = React.lazy(() => import("./TimelineChart"));

const CampaignConversionRateChart = ({ reportData }) => {
  const chartData = reportData?.data?.map((v) => {
    return {
      timestamp: v.endDate,
      personalized: (v.conversionRate * 100).toFixed(2),
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
            yAxisLabel="Conversion Rate"
            xAxisLabel="Week ending on"
          />
        </Suspense>
      )}
    </React.Fragment>
  );
};

export default CampaignConversionRateChart;
