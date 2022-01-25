import React from "react";

export const SearchBox = ({ onSearch }) => {
  const [term, setTerm] = React.useState("");
  const handleKeyDown = (event) => {
    if (event.key === "Enter") {
      if (onSearch) {
        onSearch(term);
      } else {
        console.log("No onSearch function provided.");
      }
    }
  };

  return (
    <div className="input-group input-group-lg mb-1">
      <div className="input-group-prepend">
        <span className="input-group-text" id="inputGroup-sizing-lg">
          Search:
        </span>
      </div>

      <input
        type="text"
        placeholder="Press Enter to search"
        value={term}
        onChange={(_) => setTerm(_.target.value)}
        className="form-control"
        aria-label="Large"
        aria-describedby="inputGroup-sizing-sm"
        onKeyDown={handleKeyDown}
      />
    </div>
  );
};
