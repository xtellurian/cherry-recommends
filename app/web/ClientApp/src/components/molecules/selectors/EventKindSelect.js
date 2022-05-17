import React from "react";
import { Selector } from "./Select";
import { eventKinds } from "../../../api/eventsApi";
const getEventKindOptions = () => {
  return [
    {
      label: "Custom",
      value: eventKinds.custom,
    },
    {
      label: "Behaviour",
      value: eventKinds.behaviour,
    },
    {
      label: "Consume Recommendation",
      value: eventKinds.consumeRecommendation,
    },
    {
      label: "Identify",
      value: eventKinds.identify,
    },
    {
      label: "Page View",
      value: eventKinds.pageView,
    },
    {
      label: "Purchase",
      value: eventKinds.purchase,
    },
    {
      label: "Use Promotion",
      value: eventKinds.usePromotion,
    },
  ];
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
