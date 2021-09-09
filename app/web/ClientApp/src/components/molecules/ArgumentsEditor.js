import React from "react";
import { Selector } from "./selectors/Select";

const argumentTypeOptions = [
  { label: "Numerical", value: "numerical" },
  { label: "Categorical", value: "categorical" },
];

const ArgumentRow = ({ entry, argument, onChange, onRemove }) => {
  const { commonId, argumentType, isRequired, defaultArgumentValue } = argument;
  let defaultValue = argument.defaultValue;
  if (defaultArgumentValue && typeof defaultValue === "object") {
    defaultValue = defaultArgumentValue;
  }
  return (
    <div className="row mt-1">
      <div className="col-2">
        <Selector
          placeholder="Type"
          defaultValue={
            argumentType
              ? { label: argumentType, value: argumentType }
              : argumentTypeOptions[0].value
          }
          onChange={(v) =>
            onChange(entry, entry, { ...argument, argumentType: v.value })
          }
          options={argumentTypeOptions}
        />
      </div>
      <div className="col">
        <div className="input-group">
          <div className="input-group-prepend ml-1">
            <span className="input-group-text" id="basic-addon3">
              Identifier:
            </span>
          </div>
          <input
            type="text"
            className="form-control"
            placeholder="Identifier"
            value={commonId || ""}
            onChange={(e) =>
              onChange(entry, e.target.value, {
                ...argument,
                commonId: e.target.value,
              })
            }
          />
          <div className="ml-l input-group-prepend ml-1">
            <span className="input-group-text" id="basic-addon3">
              Default Value:
            </span>
          </div>

          <input
            type={argumentType === "numerical" ? "number" : "text"}
            className="form-control"
            placeholder="Default Value"
            value={defaultValue || (argumentType === "numerical" ? 0 : "")}
            onChange={(e) =>
              onChange(entry, entry, {
                ...argument,
                defaultValue: e.target.value,
              })
            }
          />
        </div>
      </div>
      <div className="col-2">
        <div className="form-check">
          <input
            type="checkbox"
            className="form-check-input"
            checked={isRequired || false}
            onChange={(e) =>
              onChange(entry, entry, {
                ...argument,
                isRequired: e.target.checked,
              })
            }
          />
          <label className="form-check-label">Required</label>
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
export const ArgumentsEditor = ({
  initialArguments,
  onArgumentsChanged,
  placeholder,
}) => {
  const [args, setArguments] = React.useState(initialArguments || {});

  const handleChange = (oldEntry, newEntry, value) => {
    args[newEntry] = value;
    if (oldEntry !== newEntry) {
      delete args[oldEntry];
    }
    if (Object.keys(args).length === 0) {
      args[""] = "";
    }
    setArguments({ ...args });
  };
  const handleRemove = (entry) => {
    delete args[entry];
    if (Object.keys(args).length === 0) {
      args[""] = "";
    }
    setArguments({ ...args });
  };
  const handleNewEntry = () => {
    args[""] = "";
    setArguments({ ...args });
  };

  // updates the parent on changes
  const ensureDefaultValNotObject = (a) => {
    if (a.defaultValue && typeof a.defaultValue === "object") {
      a.defaultValue = a.defaultValue.value;
    }
    return a;
  };
  React.useEffect(() => {
    if (args) {
      const vals = Object.values(args);
      const vals_edited = vals.map(ensureDefaultValNotObject);
      onArgumentsChanged(vals_edited);
    }
  }, [args, onArgumentsChanged]);

  return (
    <React.Fragment>
      {Object.entries(args).length === 0 && (
        <div className="p-1">
          {placeholder || (
            <button
              className="btn btn-outline-success"
              onClick={handleNewEntry}
            >
              Add Argument
            </button>
          )}
        </div>
      )}
      {Object.entries(args).map(([k, v], i) => (
        <ArgumentRow
          key={i}
          entry={k}
          argument={v}
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
