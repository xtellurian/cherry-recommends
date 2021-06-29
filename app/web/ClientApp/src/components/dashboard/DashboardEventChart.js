import React from "react";
import {
  Area,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
  AreaChart,
} from "recharts";
import { getColours } from "../../utility/colours";

export const DashboardEventChart = ({ timeline }) => {
  if (
    !timeline ||
    !timeline.categoricalMoments ||
    timeline.categoricalMoments.length === 0
  ) {
    return <div className="text-muted text-center">No data to show.</div>;
  }
  const colours = getColours();

  // uncomment this to view the colour shceme
  //   return (
  //     <div>
  //       {colours.map((c, i) => (
  //         <div key={i} style={{ backgroundColor: `#${c}` }}>
  //           xxx
  //         </div>
  //       ))}
  //     </div>
  //   );
  return (
    <ResponsiveContainer width="95%" height={300} className="mt-3">
      <AreaChart data={timeline.categoricalMoments}>
        <XAxis
          dataKey="unixTime"
          domain={["auto", "auto"]}
          name="Time"
          tickFormatter={(t) => new Date(t).toLocaleDateString("en-AU")}
          type="number"
        />
        <Tooltip labelFormatter={(t) => new Date(t).toLocaleDateString()} />
        <YAxis />
        {timeline.categories.map((cat, i) => (
          <Area
            key={i}
            type="monotone"
            dataKey={cat}
            stackId={i + 1}
            stroke={`#${colours[colours.length % (i + 1)]}`}
            fill={`#${colours[colours.length % (i + 1)]}`}
          />
        ))}
      </AreaChart>
    </ResponsiveContainer>
  );
};
