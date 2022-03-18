import React from "react";
import { useEventTimeline } from "../../api-hooks/dataSummaryApi";
import {
  BarChart,
  Bar,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";
import { Spinner } from "../molecules/Spinner";

/**
 * @param {string} kind
 * @param {string} eventType
 */
export const EventHistoryBarChart = ({ kind, eventType }) => {
  const { result } = useEventTimeline({ kind, eventType });
  if (!kind || !eventType) {
    return (
      <div className="text-muted text-center">
        Chart will render here when kind and event type are selected.
      </div>
    );
  }

  if (!result || result.loading) {
    return <Spinner />;
  }

  return (
    <ResponsiveContainer width="95%" height={300} className="mt-3">
      <BarChart data={result.moments}>
        <XAxis
          dataKey="unixTime"
          domain={["auto", "auto"]}
          name="Month"
          tickFormatter={(unixTime) =>
            new Date(unixTime).toLocaleDateString("en-AU")
          }
          reversed={true}
        />
        <Tooltip labelFormatter={(t) => new Date(t).toLocaleDateString()} />
        <YAxis />
        <Bar dataKey="count" fill="var(--cherry-teal)" />
      </BarChart>
    </ResponsiveContainer>
  );
};
