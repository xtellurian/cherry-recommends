import React from "react";
import {
  ResponsiveContainer,
  Scatter,
  ScatterChart,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";
import { DateTimeField } from "../DateTimeField";
import { JsonView } from "../JsonView";

var groupBy = function (xs, key) {
  return xs.reduce(function (rv, x) {
    (rv[x[key]] = rv[x[key]] || []).push(x);
    return rv;
  }, {});
};

const CustomTooltip = ({ active, payload, label }) => {
  if (active && payload && payload.length) {
    const eventData = payload[0].payload;
    let properties = {};
    if (Object.keys(eventData.properties).length <= 5) {
      properties = eventData.properties;
    } else {
      for (const k of Object.keys(eventData.properties)) {
        properties[k] = eventData.properties[k];
        if (Object.keys(properties).length >= 5) {
          break;
        }
      }
    }

    return (
      <div className="custom-tooltip">
        <DateTimeField label="Timestamp" date={eventData.timestamp} />
        <div className="capitalize">
          {eventData.kind}|{eventData.eventType}
        </div>
        <JsonView data={properties} />
      </div>
    );
  }

  return null;
};

/**
 * @param {[]} events
 */
export const EventTimelineChart = ({ eventResponse }) => {
  if (
    !eventResponse ||
    eventResponse.loading ||
    eventResponse.events.length === 0
  ) {
    return (
      <div className="text-muted text-center">
        There are no events to chart.
      </div>
    );
  }

  eventResponse.events = eventResponse.events.map((k) => ({
    ...k,
    count: 1,
    unixTime: Date.parse(k.timestamp),
  }));
  let eventsByKind = groupBy(eventResponse.events, "kind");

  return (
    <ResponsiveContainer width="95%" height={100} className="mt-3">
      <ScatterChart>
        <Tooltip content={<CustomTooltip />} />;
        <XAxis
          dataKey="unixTime"
          domain={["auto", "auto"]}
          name="Time"
          tickFormatter={(t) => new Date(t).toLocaleDateString("en-AU")}
          type="number"
        />
        <YAxis dataKey="count" name="Count" hide={true} />
        {eventResponse.kinds.map((k) => {
          return (
            <Scatter key={k} data={eventsByKind[k]} name={k} fill="#8884d8" />
          );
        })}
      </ScatterChart>
    </ResponsiveContainer>
  );
};
