import React from "react";
import Tippy from "@tippyjs/react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleXmark, faCirclePlus } from "@fortawesome/free-solid-svg-icons";

import { FieldLabel } from "./FieldLabel";
import { Selector } from "./selectors/Select";
import { TextInput, commonIdValidator } from "./TextInput";

const argumentTypeOptions = [
  { label: "Numerical", value: "numerical" },
  { label: "Categorical", value: "categorical" },
];

const ArgumentRow = ({ entry, argument, onChange, onRemove }) => {
  const { commonId, argumentType, isRequired } = argument;

  return (
    <div className="border-bottom mt-4">
      <div className="d-flex align-items-center justify-content-between">
        <div className="">
          <Selector
            label="Type"
            placeholder="Type"
            inline={false}
            defaultValue={
              argumentType === "numerical"
                ? argumentTypeOptions[0]
                : argumentTypeOptions[1]
            }
            onChange={(v) =>
              onChange(entry, entry, { ...argument, argumentType: v.value })
            }
            options={argumentTypeOptions}
          />
        </div>

        <div className="">
          <TextInput
            label="Identifier"
            placeholder="Identifier"
            inline={false}
            value={commonId || ""}
            validator={commonIdValidator}
            onChange={(e) =>
              onChange(entry, e.target.value, {
                ...argument,
                commonId: e.target.value,
              })
            }
          />
        </div>

        <div className="">
          <FieldLabel type="checkbox">
            <div className="form-check w-100">
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
              <label className="form-check-label ml-1">Required</label>
            </div>
          </FieldLabel>
        </div>

        <div className="">
          <FontAwesomeIcon
            icon={faCircleXmark}
            className="cursor-pointer float-right text-danger"
            style={{ fontSize: "1.5em" }}
            onClick={() => onRemove(entry)}
          />
        </div>
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
      setArguments({});
    } else {
      setArguments({ ...args });
    }
  };
  const handleRemove = (entry) => {
    delete args[entry];
    if (Object.keys(args).length === 0) {
      setArguments({});
    } else {
      setArguments({ ...args });
    }
  };
  const handleNewEntry = () => {
    args[""] = "";
    setArguments({ ...args });
  };

  React.useEffect(() => {
    if (args) {
      const vals = Object.values(args);
      onArgumentsChanged(vals);
    }
  }, [args, onArgumentsChanged]);

  return (
    <React.Fragment>
      {Object.entries(args).length === 0 ? (
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
      ) : null}

      {Object.entries(args).map(([k, v], i) => (
        <ArgumentRow
          key={i}
          entry={k}
          argument={v}
          onChange={handleChange}
          onRemove={handleRemove}
        />
      ))}

      {Object.entries(args).length > 0 ? (
        <div className="float-right mt-4">
          <Tippy content="Add Argument" placement="left">
            <span className="d-flex align-items-center justify-content-end">
              <FontAwesomeIcon
                icon={faCirclePlus}
                className="cursor-pointer float-right text-success"
                style={{ fontSize: "1.5em" }}
                onClick={handleNewEntry}
              />
            </span>
          </Tippy>
        </div>
      ) : null}
    </React.Fragment>
  );
};
