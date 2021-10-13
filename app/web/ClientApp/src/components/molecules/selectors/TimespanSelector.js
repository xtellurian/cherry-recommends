import React from "react";
import { Selector } from "./Select";

const options = [
  {
    label: "1 Hour",
    value: "01:00:00",
  },
  {
    label: "1 Day",
    value: "1.00:00:00",
  },
  {
    label: "7 Days",
    value: "7.00:00:00",
  },
  {
    label: "14 Days",
    value: "14.00:00:00",
  },
];

export const TimespanSelector = ({ initialValue, onChange, allowNull }) => {
  if (!onChange) {
    throw new Error("onChange is a required prop");
  }
  let defaultValue = null;
  if (initialValue && typeof initialValue === "string") {
    defaultValue = options.find(
      (_) => initialValue.localeCompare(_.value) === 0
    );
  }

  return (
    <Selector
      isClearable={allowNull}
      defaultValue={defaultValue}
      onChange={(o) => onChange(o?.value)}
      options={options}
      placeholder="None"
    />
  );
};
