import React from "react";
import ReactDatePicker from "react-datepicker";

import { FieldLabel } from "./FieldLabel";

import "react-datepicker/dist/react-datepicker.css";

export const DatePicker = ({
  label,
  hint,
  required,
  optional,
  inline,
  ...props
}) => {
  return (
    <FieldLabel
      label={label}
      hint={hint}
      required={required}
      optional={optional}
      inline={inline}
    >
      <ReactDatePicker className="form-control form-field" {...props} />
    </FieldLabel>
  );
};
