import React from "react";
import {
  ResponsiveContainer,
  LineChart,
  Line,
  CartesianGrid,
  Legend,
  YAxis,
  Tooltip,
} from "recharts";
const data = [
  { name: "Text can do here", baseline: 100, cherry: 105 },
  { name: "Feature Name", baseline: 110, cherry: 120 },
  { name: "Feature Name", baseline: 105, cherry: 110 },
  { name: "Feature Name", baseline: 111, cherry: 126 },
  { name: "Feature Name", baseline: 109, cherry: 128 },
];

export default ({ label, values }) => {
  return (
    <ResponsiveContainer width="95%" height={400}>
      <LineChart data={data}>
        <Line type="monotone" dataKey="cherry" stroke="var(--cherry-pink)" />
        <Line
          type="monotone"
          dataKey="baseline"
          stroke="var(--cherry-purple)"
        />
        <CartesianGrid stroke="#ccc" />
        <YAxis />
        <Legend />
        <Tooltip
          content={
            <div className="rounded bg-light p-2">
              These data are representitive only. Real information will be
              available soon.
            </div>
          }
        />
      </LineChart>
    </ResponsiveContainer>
  );
};
