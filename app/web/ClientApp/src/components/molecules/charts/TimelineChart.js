import React from "react";
import {
  Label,
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";

// example data
// const data = [
//   {
//     timestamp: Date.parse("03 Jan 2022 00:00:00 GMT"),
//     name: "Page G",
//     uv: 3490,
//     pv: 4300,
//     amt: 2100,
//   },
// ];
const defaultToLocaleDateStringOptions = {
  day: "numeric",
  month: "short",
};
const defaultLineColors = ["#e5008a", "#280938", "#cc047c", "#00b6bf"];
const TimelineChart = ({
  containerWidth,
  containerHeight,
  data,
  toLocaleDateStringOptions,
  xAxisLabel,
  yAxisLabel,
}) => {
  const lineKeys = Object.keys(data[0]).filter((_) => _ !== "timestamp");

  return (
    <ResponsiveContainer
      width={containerWidth || "100%"}
      height={containerHeight || "100%"}
    >
      <LineChart
        width={500}
        height={300}
        data={data}
        margin={{
          top: 5,
          right: 30,
          left: 20,
          bottom: 5,
        }}
      >
        <CartesianGrid strokeDasharray="3 3" />
        <XAxis
          dataKey="timestamp"
          tickFormatter={(t) =>
            new Date(t).toLocaleDateString(
              "en-AU",
              toLocaleDateStringOptions || defaultToLocaleDateStringOptions
            )
          }
        >
          <Label value={xAxisLabel} position="insideBottomRight" offset={-15} />
        </XAxis>
        <YAxis yAxisId="left">
          <Label
            value={yAxisLabel}
            offset={0}
            position="insideBottomLeft"
            angle={-90}
          />
        </YAxis>
        {/* <YAxis yAxisId="right" orientation="right" /> */}
        <Tooltip />
        <Legend />
        {lineKeys.map((k, i) => (
          <Line
            key={k}
            yAxisId="left"
            type="monotone"
            dataKey={k}
            stroke={defaultLineColors[i]}
            activeDot={{ r: 8 }}
          />
        ))}
      </LineChart>
    </ResponsiveContainer>
  );
};

export default TimelineChart;
