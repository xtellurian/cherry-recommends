import React from "react";
import "./ToggleSwitch.css";

/*
Toggle Switch Component
Note: id, checked and onChange are required for ToggleSwitch component to function.
The props name, small, disabled and optionLabels are optional.
Usage: <ToggleSwitch id="id" checked={value} onChange={checked => setValue(checked)}} />
*/
const defaultOptionLabels = ["yes", "no"];

export const ToggleSwitch = ({
  id,
  name,
  checked,
  onChange,
  optionLabels,
  small,
  disabled,
  className,
}) => {
  if (!optionLabels) {
    optionLabels = defaultOptionLabels;
  }
  return (
    <div
      className={`toggle-switch ${className || ""} ${
        small ? " small-switch" : ""
      }`}
    >
      <input
        type="checkbox"
        name={name}
        className="toggle-switch-checkbox"
        id={id}
        checked={checked}
        onChange={(e) => onChange(e.target.checked)}
        disabled={disabled}
      />
      {id ? (
        <label className="toggle-switch-label" htmlFor={id}>
          <span
            className={
              disabled
                ? "toggle-switch-inner toggle-switch-disabled"
                : "toggle-switch-inner"
            }
            data-yes={optionLabels[0]}
            data-no={optionLabels[1]}
          />
          <span
            className={
              disabled
                ? "toggle-switch-switch toggle-switch-disabled"
                : "toggle-switch-switch"
            }
          />
        </label>
      ) : null}
    </div>
  );
};
