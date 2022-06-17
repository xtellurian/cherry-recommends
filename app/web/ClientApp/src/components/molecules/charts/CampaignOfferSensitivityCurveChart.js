import React, { Suspense } from "react";
import dayjs from "dayjs";
import {
  Bar,
  BarChart,
  CartesianGrid,
  Label,
  Legend,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";
import { Spinner } from "../Spinner";

const defaultColors = ["#e5008a", "#00dae5", "#280938", "#cc047c", "#00b6bf"];
const renderTooltip = ({ active, payload, label }) => {
  if (active && payload && payload.length) {
    return (
      <div
        className="recharts-default-tooltip"
        style={{
          margin: "0px",
          padding: "10px",
          backgroundColor: "rgb(255, 255, 255)",
          border: "1px solid rgb(204, 204, 204)",
          whiteSpace: "nowrap",
        }}
      >
        <p className="recharts-tooltip-label" style={{ margin: "0px" }}>
          {label}
        </p>
        <ul
          className="recharts-tooltip-item-list"
          style={{ padding: "0px", margin: "0px" }}
        >
          <li
            className="recharts-tooltip-item"
            style={{
              display: "block",
              paddingTop: "4px",
              paddingBottom: "4px",
              color: payload[0].fill,
            }}
          >
            <span class="recharts-tooltip-item-name">{payload[0].name}</span>
            <span class="recharts-tooltip-item-separator"> : </span>
            <span class="recharts-tooltip-item-value">{payload[0].value}</span>
            <span class="recharts-tooltip-item-unit">{payload[0].unit}</span>
          </li>
          <li
            className="recharts-tooltip-item"
            style={{
              display: "block",
              paddingTop: "4px",
              paddingBottom: "4px",
              color: payload[1].fill,
            }}
          >
            <span class="recharts-tooltip-item-name">{payload[1].name}</span>
            <span class="recharts-tooltip-item-separator"> : </span>
            <span class="recharts-tooltip-item-unit">{payload[1].unit}</span>
            <span class="recharts-tooltip-item-value">{payload[1].value}</span>
          </li>
        </ul>
      </div>
    );
  }

  return null;
};
const CampaignOfferSensitivityCurveChart = ({ reportData }) => {
  const chartData = reportData?.data?.map((v, i) => {
    return {
      Promotion: v.promotionName,
      ARPO: v.meanGrossRevenue.toFixed(2),
      "Conversion Rate": (v.conversionRate * 100).toFixed(2),
    };
  });
  const dataSinceDate = dayjs(
    reportData?.dataSinceDate?.substring(0, 10),
    "YYYY-MM-DD"
  );
  return (
    <React.Fragment>
      {reportData.loading ? (
        <Spinner />
      ) : (
        <Suspense fallback={<Spinner />}>
          <ResponsiveContainer width="100%" aspect={2}>
            <BarChart
              width={500}
              height={300}
              data={chartData}
              margin={{
                top: 10,
                right: 30,
                left: 20,
                bottom: 5,
              }}
            >
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="Promotion">
                <Label
                  value={`Data since ${dataSinceDate.format("MMM DD")}`}
                  position="insideBottomRight"
                  offset={-15}
                />
              </XAxis>
              <YAxis
                yAxisId="left"
                dataKey="Conversion Rate"
                domain={[0, 100]}
                orientation="left"
                axisLine={{ stroke: defaultColors[0] }}
                allowDataOverflow
                allowDecimals
              >
                <Label
                  value={"Conversion Rate (%)"}
                  position="center"
                  style={{ fill: defaultColors[0] }}
                  dx={-20}
                  angle={-90}
                />
              </YAxis>
              <YAxis
                yAxisId="right"
                dataKey="ARPO"
                domain={[0, "auto"]}
                orientation="right"
                axisLine={{ stroke: defaultColors[1] }}
                allowDataOverflow
                allowDecimals
              >
                <Label
                  value={"Average Revenue per Offer ($)"}
                  position="center"
                  style={{ fill: defaultColors[1] }}
                  dx={20}
                  angle={-90}
                />
              </YAxis>
              <Tooltip content={renderTooltip} />
              <Legend />
              <Bar
                yAxisId="left"
                dataKey={"Conversion Rate"}
                fill={defaultColors[0]}
                unit="%"
              />
              <Bar
                yAxisId="right"
                dataKey={"ARPO"}
                fill={defaultColors[1]}
                unit="$"
              />
            </BarChart>
          </ResponsiveContainer>
        </Suspense>
      )}
    </React.Fragment>
  );
};

export default CampaignOfferSensitivityCurveChart;
