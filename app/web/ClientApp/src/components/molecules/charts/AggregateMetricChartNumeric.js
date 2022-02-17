import React from "react";
import dayjs from "dayjs";
import {
  ResponsiveContainer,
  LineChart,
  Line,
  CartesianGrid,
  Legend,
  YAxis,
  Tooltip,
  XAxis,
} from "recharts";
import { getFirstLastOfTheWeek } from "../../../utility/utility";

const xTickFormatter = (value, index) => {
  const { firstday, lastday } = getFirstLastOfTheWeek(new Date());
  const lastOfWeek = dayjs(lastday);
  const valueDate = dayjs(value.substring(0, 10), "YYYY-MM-DD");
  if (lastOfWeek.isSame(valueDate, "day")) {
    return "Present";
  }
  const tick = valueDate.format("MMM DD");
  return tick;
};

const xLabelFormatter = (payload) => {
  const valueDate = dayjs(payload.lastOfWeek.substring(0, 10), "YYYY-MM-DD");
  return `Week ending in ${valueDate.format("MMM DD")}`;
};

const renderTooptip = ({ active, payload, label }) => {
  if (active && payload && payload.length) {
    const customerCount = payload[0].payload.weeklyDistinctCustomerCount || 0;
    return (
      <div className="amc-tooltip">
        <label className="font-weight-bold d-block mb-1">
          {xLabelFormatter(payload[0].payload)}
        </label>
        <label className="mb-0">Customers</label>
        <p className="text-left font-weight-bold mb-1">{customerCount}</p>
        <label className="mb-0">Mean</label>
        <p className="text-left font-weight-bold mb-1">
          {parseFloat(payload[0].value).toFixed(2)}
        </p>
      </div>
    );
  }

  return null;
};

export default ({ metric, data }) => {
  if (data && data.loading) {
    return <div className="text-muted text-center">Loading...</div>;
  }
  if (!data || (data && !data.length)) {
    return (
      <div className="text-muted text-center">
        There are no values to chart.
      </div>
    );
  }

  return (
    <React.Fragment>
      <h5 className="text-center">Mean Weekly - {metric.name}</h5>
      <ResponsiveContainer width="95%" aspect={2}>
        <LineChart
          data={data}
          margin={{
            top: 30,
            right: 30,
            left: 30,
            bottom: 30,
          }}
        >
          <CartesianGrid strokeDasharray="3 3" stroke="#ccc" />
          <XAxis
            dataKey="lastOfWeek"
            type="category"
            axisLine={false}
            tick={{
              stroke: "var(--cherry-purple)",
              fontSize: 11,
              strokeWidth: 0.8,
            }}
            tickFormatter={xTickFormatter}
            scale="auto"
            angle={0}
            interval={0}
            allowDuplicatedCategory={false}
            label={{ value: "Week", angle: 0, position: "bottom", offset: 8 }}
          />
          <YAxis
            dataKey="weeklyMeanNumericValue"
            type="number"
            axisLine={false}
            allowDecimals={false}
            allowDuplicatedCategory={false}
          />
          <Line
            type="monotone"
            dataKey="weeklyMeanNumericValue"
            stroke="var(--cherry-pink)"
            activeDot={{ r: 8 }}
          />
          <Tooltip content={renderTooptip} />
        </LineChart>
      </ResponsiveContainer>
    </React.Fragment>
  );
};
