import React from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

export const ScheduleUtil = ({
  header,
  onOptionChanged,
  onDateChanged,
  expiryDateEnabled,
  expiryDate,
}) => {
  return (
    <React.Fragment>
      <div className="mt-2">
        {header}
        <div className="text-left">
          <input
            type="radio"
            value={false}
            name="schedule"
            checked={expiryDateEnabled === false}
            onChange={(v) => onOptionChanged(false)}
          />
          <span> Run continuously</span>
        </div>
        <div className="text-left">
          <input
            type="radio"
            value={true}
            name="schedule"
            checked={expiryDateEnabled === true}
            onChange={(v) => onOptionChanged(true)}
          />
          <span> Set expiry date (UTC)</span>
          <div className="ml-3 mt-1">
            <DatePicker
              className="form-control"
              readOnly={!expiryDateEnabled}
              selected={expiryDate ? expiryDate : new Date()}
              onChange={(date) => {
                onDateChanged(date);
              }}
            />
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};
