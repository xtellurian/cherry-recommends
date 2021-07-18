import React from "react";
import { BackButton, EmptyList, Subtitle, Title } from "../../molecules";
import {
  ResponsiveContainer,
  LineChart,
  CartesianGrid,
  XAxis,
  YAxis,
  Tooltip,
  Legend,
  Line,
} from "recharts";

const PayloadToDates = ({ payload }) => {
  const start = new Date(payload.start);
  const end = new Date(payload.end);
  return (
    <React.Fragment>
      <p>
        {start.toLocaleDateString()} to {end.toLocaleDateString()}
      </p>
    </React.Fragment>
  );
};

const CustomTooltip = ({ active, payload, label }) => {
  if (active && payload && payload.length) {
    return (
      <div className="bg-light p-2">
        <p className="label">{`Iteration ${label} : ${payload[0].value}`}</p>
        <PayloadToDates payload={payload[0].payload} />
      </div>
    );
  }

  return null;
};

// colors for the lines in the graph
const colors = ["#1c789e", "8884d8", "82ca9d"];

export const TargetVariableValuesUtility = ({
  recommender,
  rootPath,
  targetVariableValues,
}) => {
  const uniqueNames = [
    ...new Set(targetVariableValues.map((item) => item.name)),
  ];
  const data = targetVariableValues.map((tv) => ({
    ...tv,
    [tv.name]: tv.value,
  }));

  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to={`${rootPath}/detail/${recommender.id}`}
      >
        Back to Recommender
      </BackButton>
      <Title>Target Variable</Title>
      <Subtitle>{recommender.name || "..."}</Subtitle>
      <hr />

      {targetVariableValues.length === 0 && (
        <EmptyList>
          Target Variable information is being collected. Check back later.
        </EmptyList>
      )}

      {targetVariableValues.length > 0 && (
        <ResponsiveContainer width="99%" height={300}>
          <LineChart
            width={730}
            height={250}
            data={data}
            margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
          >
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="version" />
            <YAxis />
            <Tooltip content={<CustomTooltip />} />
            <Legend />

            {uniqueNames.map((n, i) => (
              <Line
                key={n}
                type="monotone"
                dataKey={n}
                stroke={colors[i % colors.length]}
              />
            ))}
          </LineChart>
        </ResponsiveContainer>
      )}
    </React.Fragment>
  );
};
