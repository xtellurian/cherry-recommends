import React from "react";
import { DebounceInput } from "react-debounce-input";

export const SearchBox = ({ onSearch }) => {
  return (
    <div className="input-group input-group-lg mb-1">
      <div className="input-group-prepend">
        <span className="input-group-text" id="inputGroup-sizing-lg">
          Search:
        </span>
      </div>

      <DebounceInput
        className="form-control"
        aria-label="Large"
        aria-describedby="inputGroup-sizing-sm"
        placeholder="Type text to search"
        minLength={1}
        debounceTimeout={500}
        onChange={(event) => onSearch(event.target.value)}
      />
    </div>
  );
};
