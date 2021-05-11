import React from "react";
import {
  LineChart,
  XAxis,
  Tooltip,
  CartesianGrid,
  Line,
  Legend,
  YAxis,
} from "recharts";

export const Chart = ({ iterations }) => {
  return (
    <LineChart
      width={400}
      height={400}
      data={iterations}
      margin={{ top: 5, right: 20, left: 10, bottom: 5 }}
    >
      <Legend />
      <XAxis dataKey="order" label="Iteration" />
      <Tooltip />

      <CartesianGrid stroke="#f5f5f5" />
      <Line
        type="monotone"
        name="Offers Presented"
        dataKey="offersMade"
        stroke="#ff7300"
        yAxisId={0}
      />
      <Line
        name="Mean Presentation Income"
        type="monotone"
        dataKey="offerIncome"
        stroke="#387908"
        yAxisId={1}
      />
    </LineChart>
  );
};
