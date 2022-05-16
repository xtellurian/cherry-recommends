import React from "react";
import { Selector } from "./Select";

const getEventKindOptions = () => {
  const result = [
    {
      label: "Custom",
      value: "custom",
    },
    {
      label: "Behaviour",
      value: "behaviour",
    },
    {
      label: "Consume Recommendation",
      value: "consumeRecommendation",
    },
  ];
  return result;
};

export const EventKindSelect = ({
  label,
  required,
  defaultValue,
  placeholder,
  onSelected,
}) => {
  const options = getEventKindOptions();
  // let starting = null;
  // if (defaultValue !== undefined) {
  //   starting = {
  //     label: defaultValue,
  //     value: defaultValue,
  //   };
  // }
  const [selectedOption, setSelectedOption] = React.useState(null);
  React.useEffect(() => {
    if (selectedOption && selectedOption.value) {
      onSelected(selectedOption.value);
    }
  }, [selectedOption]);

  return (
    <Selector
      label={label}
      required={required}
      placeholder={placeholder}
      defaultValue={selectedOption}
      onChange={setSelectedOption}
      options={options}
    />
  );
};
