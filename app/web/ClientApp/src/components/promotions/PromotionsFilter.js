import React from "react";

import { useOutsideClick } from "../../utility/utility";
import { benefitTypeOptons, promotionTypeOptons } from "./CreatePromotion";

import "./PromotionsFilter.css";

const addedByOptions = [
  { value: 1, label: "Last week" },
  { value: 4, label: "Last month" },
  { value: 12, label: "3 months" },
  { value: 24, label: "6 months" },
];

const Dropdown = ({
  label,
  options,
  values,
  multiple,
  className = "",
  onChange,
}) => {
  const [active, setActive] = React.useState(false);
  const wrapperRef = React.useRef(null);

  useOutsideClick({ ref: wrapperRef, onClick: () => setActive(false) });

  const handleChange = (e) => {
    const newValue = Number(e.target.value) || e.target.value;
    let newValues = [...values, newValue];

    if (!e.target.checked) {
      newValues = [...values].filter((value) => value !== newValue);
    }

    if (!multiple) {
      newValues = e.target.checked ? [newValue] : [];
    }

    onChange(e, { values: newValues });
  };

  const toggleOptions = () => {
    setActive((oldOpen) => !oldOpen);
  };

  return (
    <div ref={wrapperRef} className={`dropdown disable-select ${className}`}>
      <span
        className={`${!active && !values.length ? "text-black-50" : ""}`}
        onClick={toggleOptions}
      >
        {label}
      </span>

      {active ? (
        <div className="dropdown-content card px-3 py-2 mt-2">
          {options.map((option) => (
            <div key={option.value}>
              <input
                id={option.value}
                className="mr-2"
                type="checkbox"
                name={option.value}
                value={option.value}
                checked={values.includes(option.value)}
                onChange={handleChange}
              />
              <label htmlFor={option.value}>{option.label}</label>
            </div>
          ))}
        </div>
      ) : null}
    </div>
  );
};

export const PromotionsFilter = ({ filters, setFilters }) => {
  const [active, setActive] = React.useState(false);

  return (
    <div className="filters mt-3 mx-2">
      <label
        className="filters-label disable-select"
        onClick={() => setActive((oldActive) => !oldActive)}
      >
        Filters
      </label>
      {active ? (
        <React.Fragment>
          <Dropdown
            label="Promotion Type"
            multiple={true}
            options={promotionTypeOptons}
            values={filters.promotionType}
            onChange={(e, { values }) => {
              setFilters({ ...filters, promotionType: values });
            }}
          />
          <Dropdown
            label="Benefit Type"
            multiple={true}
            options={benefitTypeOptons}
            values={filters.benefitType}
            onChange={(e, { values }) => {
              setFilters({ ...filters, benefitType: values });
            }}
          />
          <Dropdown
            label="Added By"
            multiple={true}
            options={addedByOptions}
            values={filters.addedBy}
            onChange={(e, { values }) => {
              setFilters({ ...filters, addedBy: values });
            }}
          />
        </React.Fragment>
      ) : null}
    </div>
  );
};
