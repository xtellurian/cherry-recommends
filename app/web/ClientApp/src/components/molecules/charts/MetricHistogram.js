import React from "react";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";

import "./AggregateMetricChart.css";

const MAX_BINS = 12;

const CustomTooltip = ({ active, payload, label }) => {
  if (active && payload && payload.length) {
    const data = payload[0];
    return (
      <div className="amc-tooltip">
        <label className="d-block mb-1 text-center">{label}</label>
        <label className="mb-0">{data.name}</label>
        <p className="text-left font-weight-bold mb-1">
          {data.payload[data.dataKey]}
        </p>
      </div>
    );
  }

  return null;
};

export const MetricHistogram = ({ metric, data, xAxis, yAxis }) => {
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

  const _data = data.slice(0, MAX_BINS);

  return (
    <React.Fragment>
      <h5 className="text-center">{metric.name}</h5>
      <ResponsiveContainer width="95%" aspect={2}>
        <BarChart
          data={_data}
          barCategoryGap={0}
          barSize={40}
          margin={{
            top: 30,
            right: 30,
            left: 30,
            bottom: 30,
          }}
        >
          <XAxis
            dataKey={xAxis.dataKey}
            scale="point"
            padding={{ left: 60, right: 60 }}
          />
          <YAxis />
          <CartesianGrid strokeDasharray="3 3" />
          <Tooltip content={<CustomTooltip />} />
          <Legend wrapperStyle={{ bottom: 0 }} />
          <Bar name={yAxis.name} dataKey={yAxis.dataKey} fill="#8884d8" />
        </BarChart>
      </ResponsiveContainer>
    </React.Fragment>
  );
};
