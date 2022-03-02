import React from "react";
import { ResponsiveContainer, BarChart, Bar, YAxis, Tooltip } from "recharts";

const CustomTooltip = ({ active, payload, label }) => {
  if (active && payload && payload.length) {
    const data = payload[0];

    return (
      <div className="amc-tooltip" style={{ minWidth: 100 }}>
        <label className="mb-0">Highest Value</label>
        <p className="text-left font-weight-bold mb-1">{data.payload.high}</p>
        <label className="mb-0">Lowest Value</label>
        <p className="text-left font-weight-bold mb-1">{data.payload.low}</p>
        <label className="mb-0">Quartile 1</label>
        <p className="text-left font-weight-bold mb-1">{data.payload.open}</p>
        <label className="mb-0">Quartile 3</label>
        <p className="text-left font-weight-bold mb-1">{data.payload.close}</p>
        <label className="mb-0">Median</label>
        <p className="text-left font-weight-bold mb-1">{data.payload.median}</p>
      </div>
    );
  }

  return null;
};

const Candlestick = ({
  fill,
  x,
  y,
  width,
  height,
  low,
  high,
  median,
  openClose: [open, close],
}) => {
  const isGrowing = open < close;
  const ratio = Math.abs(height / (open - close));
  const medianRatio = y + (close - median) * ratio;

  return (
    <g stroke={fill} fill={isGrowing ? fill : "none"} strokeWidth="2">
      {/* bottom line */}
      {isGrowing ? (
        <path
          d={`
            M ${x + width / 2}, ${y + height}
            v ${(open - low) * ratio}
          `}
        />
      ) : (
        <path
          d={`
            M ${x + width / 2}, ${y}
            v ${(close - low) * ratio}
          `}
        />
      )}

      {/* top line */}
      {isGrowing ? (
        <path
          d={`
            M ${x + width / 2}, ${y}
            v ${(close - high) * ratio}
          `}
        />
      ) : (
        <path
          d={`
            M ${x + width / 2}, ${y + height}
            v ${(open - high) * ratio}
          `}
        />
      )}

      {/* bar */}
      <path
        d={`
          M ${x},${y}
          L ${x},${y + height}
          L ${x + width},${y + height}
          L ${x + width},${y}
          L ${x},${y}
        `}
      />

      {/* median line */}
      <path
        strokeWidth="1"
        stroke="#C10B75"
        d={`
          M ${x},${medianRatio}
          L ${x + width},${medianRatio}
        `}
      />
    </g>
  );
};

const prepareData = (data) => {
  if (data?.length !== 4) {
    return [];
  }

  return [
    {
      high: data[3]?.binFloor,
      low: data[0]?.binFloor,
      open: data[1]?.binFloor,
      close: data[2]?.binFloor,
      openClose: [data[1]?.binFloor, data[2]?.binFloor],
      median: (data[1]?.binFloor + data[2]?.binFloor) / 2,
    },
  ];
};

export const MetricQuartilesThumbnailChart = ({ metric, data }) => {
  const quartileData = prepareData(data);
  const minValue = quartileData[0]?.low;
  const maxValue = quartileData[0]?.high;

  if (!quartileData?.length || metric.valueType !== "numeric") {
    return null;
  }

  return (
    <ResponsiveContainer width="40%">
      <BarChart
        data={quartileData}
        margin={{
          top: 30,
        }}
      >
        <YAxis domain={[minValue - 1, maxValue + 1]} />
        <Bar
          dataKey="openClose"
          fill="#280838"
          barSize={40}
          shape={<Candlestick />}
        />
      </BarChart>
    </ResponsiveContainer>
  );
};

export const MetricQuartilesChart = ({ metric, data }) => {
  if (data && data.loading) {
    return <div className="text-muted text-center">Loading...</div>;
  }

  if (!data || (data && !data.length) || metric.valueType !== "numeric") {
    return (
      <div className="text-muted text-center">
        There are no values to chart.
      </div>
    );
  }

  const quartileData = prepareData(data);
  const minValue = quartileData[0]?.low;
  const maxValue = quartileData[0]?.high;

  if (!quartileData?.length) {
    return (
      <div className="text-muted text-center">
        There are incomplete values to chart.
      </div>
    );
  }

  return (
    <React.Fragment>
      <h5 className="text-center">{metric.name}</h5>
      <ResponsiveContainer width="25%" aspect={1} className="mx-auto my-5">
        <BarChart
          data={quartileData}
          margin={{
            top: 30,
            right: 30,
            left: 30,
            bottom: 30,
          }}
        >
          <YAxis domain={[minValue - 1, maxValue + 1]} />
          <Tooltip
            content={<CustomTooltip />}
            position={{ x: 250, y: 50 }}
            cursor={false}
          />
          <Bar
            dataKey="openClose"
            fill="#280838"
            barSize={40}
            shape={<Candlestick />}
          />
        </BarChart>
      </ResponsiveContainer>
    </React.Fragment>
  );
};
