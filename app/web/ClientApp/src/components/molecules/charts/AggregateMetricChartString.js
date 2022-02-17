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
import {
  generateRandomHexColor,
  getFirstLastOfTheWeek,
} from "../../../utility/utility";

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
    return (
      <div className="amc-tooltip">
        <label>{xLabelFormatter(payload[0].payload)}</label>
        <ul>
          {payload.map((r, index) => {
            return (
              <li key={r.name}>
                <span style={{ color: r.stroke }}>{r.name}: </span>
                {r.value}
              </li>
            );
          })}
        </ul>
      </div>
    );
  }

  return null;
};

const reduceData = (data) => {
  return data.reduce((previousValue, currentValue) => {
    const {
      stringValue,
      firstOfWeek,
      lastOfWeek,
      weeklyValueCount,
      weeklyDistinctCustomerCount,
    } = currentValue;
    const _index = previousValue.findIndex(
      (r) => r.firstOfWeek === firstOfWeek
    );
    // For categorical metric values we should use weeklyDistinctCustomerCount
    if (_index > -1) {
      return previousValue.map((r, i) => {
        if (i === _index) {
          return {
            ...r,
            [stringValue]: (r[stringValue] || 0) + weeklyDistinctCustomerCount,
          };
        }

        return r;
      });
    } else {
      return [
        ...previousValue,
        {
          firstOfWeek,
          lastOfWeek,
          [stringValue]: weeklyDistinctCustomerCount,
        },
      ];
    }
  }, []);
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

  const _ = reduceData(data);
  const keys = [...new Set(data.flatMap((r) => r.stringValue))];
  const lines = keys.map((k, i) => {
    return (
      <Line
        key={k}
        dataKey={k}
        type="monotone"
        stroke={generateRandomHexColor()}
        activeDot={{ r: 8 }}
      />
    );
  });
  return (
    <React.Fragment>
      <h5 className="text-center">Weekly Count - {metric.name}</h5>
      <ResponsiveContainer width="95%" aspect={2}>
        <LineChart
          data={_}
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
            type="number"
            axisLine={false}
            allowDecimals={false}
            allowDuplicatedCategory={false}
          />
          {lines}
          <Tooltip content={renderTooptip} />
          <Legend
            verticalAlign="top"
            layout="vertical"
            align="right"
            wrapperStyle={{
              paddingLeft: "10px",
            }}
          />
        </LineChart>
      </ResponsiveContainer>
    </React.Fragment>
  );
};
