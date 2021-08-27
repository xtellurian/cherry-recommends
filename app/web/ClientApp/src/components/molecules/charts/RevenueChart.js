import React from "react";
import dayjs from "dayjs";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";

const convertActions = (actions) => {
  return actions
    .map((a) => ({
      timestamp: new Date(a.timestamp).valueOf(),
      revenue: a.associatedRevenue,
      eventId: a.eventId,
      trackedUserId: a.trackedUserId,
    }))
    .sort((a, b) => a.timestamp - b.timestamp);
};

const RevenueChartTooptip = ({ active, payload, label }) => {
  if (active && payload && payload.length) {
    const ts = dayjs(payload[0].payload.timestamp).format("DD-MM-YYYY HH:mm");
    const revenue = payload[0].payload.revenue;
    const eventId = payload[0].payload.eventId;
    const trackedUserId = payload[0].payload.trackedUserId;
    return (
      <div className="bg-light p-2">
        <p>{ts}</p>
        <p>Event ID: {eventId}</p>
        <p>Revenue: {revenue}</p>
        <p>Tracked User ID: {trackedUserId}</p>
      </div>
    );
  }

  return null;
};

const RevenueChart = ({ actions }) => {
  const revenueData = convertActions(actions);
  return (
    <React.Fragment>
      <ResponsiveContainer width="100%" height={400} className="mb-5">
        <LineChart
          width={500}
          height={300}
          data={revenueData}
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
            tickFormatter={(unixTime) => dayjs(unixTime).format("D-MM")}
          />
          <YAxis label="$" />
          <Tooltip content={<RevenueChartTooptip />} />
          <Legend />
          <Line
            type="monotone"
            dataKey="revenue"
            stroke="#1e6ec2"
            activeDot={{ r: 8 }}
          />
          {/* <Line type="monotone" dataKey="uv" stroke="#82ca9d" /> */}
        </LineChart>
      </ResponsiveContainer>
    </React.Fragment>
  );
};

export default RevenueChart;
