import React from "react";
import { Subtitle } from "./PageHeadings";

const PropertyRow = ({ entry, value, onChange, onRemove }) => {
  return (
    <div className="row mt-1">
      <div className="col">
        <div className="input-group">
          <div className="input-group-prepend ml-1">
            <span className="input-group-text" id="basic-addon3">
              Key:
            </span>
          </div>
          <input
            type="text"
            className="form-control"
            placeholder="Entry"
            value={entry}
            onChange={(e) => onChange(entry, e.target.value, value)}
          />
          <div className="ml-l input-group-prepend ml-1">
            <span className="input-group-text" id="basic-addon3">
              Value:
            </span>
          </div>
          <input
            type="text"
            className="form-control"
            placeholder="Value"
            value={value}
            onChange={(e) => onChange(entry, entry, e.target.value)}
          />
        </div>
      </div>
      <div className="col-1">
        <button
          onClick={() => onRemove(entry)}
          className="btn btn-outline-danger btn-block"
        >
          X
        </button>
      </div>
    </div>
  );
};
export const PropertiesEditor = ({
  initialProperties,
  onPropertiesChanged,
}) => {
  const [properties, setProperties] = React.useState(initialProperties);
  const handleChange = (oldEntry, newEntry, value) => {
    properties[newEntry] = value;
    if (oldEntry !== newEntry) {
      delete properties[oldEntry];
    }
    if (Object.keys(properties).length === 0) {
      properties[""] = "";
    }
    setProperties({ ...properties });
  };
  const handleRemove = (entry) => {
    delete properties[entry];
    if (Object.keys(properties).length === 0) {
      properties[""] = "";
    }
    setProperties({ ...properties });
  };
  const handleNewEntry = () => {
    properties[""] = "";
    setProperties({ ...properties });
  };

  // updates the parent on changes
  React.useEffect(() => {
    if (properties) {
      onPropertiesChanged(properties);
    }
  }, [properties, onPropertiesChanged]);

  return (
    <React.Fragment>
      <Subtitle>Property Editor</Subtitle>
      {Object.entries(properties).map(([k, v], i) => (
        <PropertyRow
          key={i}
          entry={k}
          value={v}
          onChange={handleChange}
          onRemove={handleRemove}
        />
      ))}
      <div className="row mt-2 justify-content-end">
        <div className="col-1">
          <button
            onClick={handleNewEntry}
            className="btn btn-outline-success btn-block"
          >
            +
          </button>
        </div>
      </div>
    </React.Fragment>
  );
};
