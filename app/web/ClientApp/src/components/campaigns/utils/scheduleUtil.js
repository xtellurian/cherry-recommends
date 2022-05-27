import React from "react";

import { DatePicker } from "../../molecules";
import { FieldLabel } from "../../molecules/FieldLabel";

export const ScheduleUtil = ({
  onOptionChanged,
  onDateChanged,
  expiryDateEnabled,
  expiryDate,
  label,
}) => {
  return (
    <FieldLabel label={label} labelPosition="top">
      <div className="form-field w-100">
        <div className="row">
          <div className="d-flex align-items-center col-12">
            <input
              type="radio"
              value={false}
              name="schedule"
              checked={expiryDateEnabled === false}
              onChange={(v) => onOptionChanged(false)}
            />
            <span className="ml-2">Run continuously</span>
          </div>
          <div className="d-flex align-items-center col-12 mt-2">
            <input
              type="radio"
              value={true}
              name="schedule"
              checked={expiryDateEnabled === true}
              onChange={(v) => onOptionChanged(true)}
            />
            <span className="ml-2">Set expiry date (UTC)</span>
          </div>
          {expiryDateEnabled ? (
            <div className="col-12 mt-3">
              <DatePicker
                readOnly={!expiryDateEnabled}
                selected={expiryDate ? expiryDate : new Date()}
                onChange={(date) => {
                  onDateChanged(date);
                }}
              />
            </div>
          ) : null}
        </div>
      </div>
    </FieldLabel>
  );
};
