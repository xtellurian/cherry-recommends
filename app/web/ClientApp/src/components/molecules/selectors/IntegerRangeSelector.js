import React from "react";
import { Selector } from "./Select";

const getRangeOptions = (min, max) => {
  const result = [];
  if (min === undefined) {
    min = 0;
  }
  if (max === undefined) {
    max = 1;
  }

  for (let i = min; i <= max; i++) {
    result.push({
      label: `${i}`,
      value: i,
    });
  }
  return result;
};

export const IntegerRangeSelector = ({
  max,
  min,
  defaultValue,
  placeholder,
  onSelected,
  label,
  required,
  optional,
}) => {
  const options = getRangeOptions(min, max);
  let starting = null;
  if (
    defaultValue !== undefined ||
    (defaultValue !== null && !isNaN(parseInt(defaultValue)))
  ) {
    starting = {
      label: defaultValue,
      value: parseInt(defaultValue),
    };
  }
  const [selectedOption, setSelectedOption] = React.useState(null);
  React.useEffect(() => {
    onSelected(selectedOption?.value);
  }, [selectedOption]);

  return (
    <Selector
      label={label}
      required={required}
      optional={optional}
      placeholder={placeholder}
      defaultValue={selectedOption}
      options={options}
      onChange={setSelectedOption}
    />
  );
};
