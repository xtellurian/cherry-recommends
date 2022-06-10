import React, { Suspense } from "react";
import { Spinner } from "../Spinner";

const Chart = React.lazy(() => import("./TimelineChart"));

const dummyData = [
  {
    timestamp: Date.parse("01 Jan 2022 00:00:00 GMT"),
    uv: 2390,
    pv: 3800,
    amt: 2500,
  },
  {
    timestamp: Date.parse("02 Jan 2022 00:00:00 GMT"),
    uv: 3490,
    pv: 4300,
    amt: 2100,
  },
  {
    timestamp: Date.parse("03 Jan 2022 00:00:00 GMT"),
    name: "Page G",
    uv: 3490,
    pv: 4300,
    amt: 2100,
  },
];

const CampaignARPOChart = ({ reportData }) => {
  const chartData = reportData?.data
    ? reportData.data.map((v, i) => {
        return {
          timestamp: v.endDate,
          Overall: v.meanGrossRevenue.toFixed(2),
          Control: v.baselineMeanGrossRevenue.toFixed(2),
        };
      })
    : dummyData;
  return (
    <React.Fragment>
      {reportData.loading ? (
        <Spinner />
      ) : (
        <Suspense fallback={<Spinner />}>
          <Chart
            containerHeight={300}
            data={chartData}
            yAxisLabel="Average Revenue per Offer ($)"
            xAxisLabel="Week ending on"
            yAxisDomain={[0, "auto"]}
          />
        </Suspense>
      )}
    </React.Fragment>
  );
};

export default CampaignARPOChart;
