import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faCircleXmark,
  faCirclePlus,
  faCircleInfo,
} from "@fortawesome/free-solid-svg-icons";

import { EmptyList } from "./empty/EmptyList";
import { Typography } from "./Typography";
import { TextInput } from "./TextInput";
import { SuggestedProperties } from "../promotions/SuggestedProperties";
import { HintTippy } from "./FieldLabel";

const PropertyRow = ({
  id,
  entry,
  value,
  isLast,
  hint,
  onChange,
  onRemove,
  onAdd,
}) => {
  const [state, setState] = React.useState({
    key: entry,
    value,
  });

  const handleUpdate = () => {
    onChange(id, state.key, state.value);
  };

  React.useEffect(() => {
    if (state.key !== entry || state.value !== value) {
      setState({ key: entry, prevKey: state.prevKey, value: value });
    }
  }, [entry, value]);

  return (
    <div className="row">
      <div className="col">
        <HintTippy value={hint}>
          <div className="relative">
            <TextInput
              placeholder="Enter property name"
              value={state.key}
              disabled={hint}
              onBlur={handleUpdate}
              onChange={(e) =>
                setState({
                  key: e.target.value,
                  value: state.value,
                })
              }
            />
            {hint ? (
              <FontAwesomeIcon
                icon={faCircleInfo}
                className="text-secondary"
                style={{
                  position: "absolute",
                  top: "0.95em",
                  right: "2.25em",
                }}
                fontSize={12}
              />
            ) : null}
          </div>
        </HintTippy>
      </div>
      <div className="col">
        <TextInput
          placeholder="Enter property value"
          value={state.value}
          onBlur={handleUpdate}
          onChange={(e) =>
            setState({
              key: state.key,
              value: e.target.value,
            })
          }
        />
      </div>
      <div className="col-1">
        {isLast ? (
          <FontAwesomeIcon
            icon={faCirclePlus}
            className="cursor-pointer float-right text-success mt-2 ml-2"
            style={{ fontSize: "1.5em" }}
            onClick={onAdd}
          />
        ) : null}
        <FontAwesomeIcon
          icon={faCircleXmark}
          className="cursor-pointer float-right text-danger mt-2"
          style={{ fontSize: "1.5em" }}
          onClick={() => onRemove(id)}
        />
      </div>
    </div>
  );
};

function maxValue(arr) {
  if (!arr || arr.length === 0) {
    return 0;
  }
  let max = arr[0];

  for (let val of arr) {
    if (val > max) {
      max = val;
    }
  }
  return max;
}

const dictToList = (properties) => {
  return Object.keys(properties).map((k, i) => ({
    key: k,
    value: properties[k],
    id: i,
  }));
};

const listToDict = (propertyList) => {
  const result = {};
  for (const p of propertyList) {
    result[p.key] = p.value;
  }
  return result;
};

export const PropertiesEditor = ({
  label,
  initialProperties,
  onPropertiesChanged,
  placeholder,
  suggestions,
}) => {
  const [properties, setProperties] = React.useState(initialProperties || {});
  const [propertyList, setPropertyList] = React.useState(
    dictToList(initialProperties || {})
  );

  const handleChange = (id, key, value) => {
    const prop = propertyList.find((_) => _.id === id);
    if (prop) {
      prop.key = key;
      prop.value = value;
      setPropertyList([...propertyList]);
    } else {
      console.warn("warning: couldn't find property ID");
    }
  };

  const handleRemove = (id) => {
    const propIndex = propertyList.findIndex((_) => _.id === id);
    if (propIndex > -1) {
      propertyList.splice(propIndex, 1);
      setPropertyList([...propertyList]);
    }
  };

  const handleNewEntry = (e, customValue) => {
    // check if the last one is empty, and don't add if so
    if (propertyList && propertyList.length > 0) {
      const lastItem = propertyList[propertyList.length - 1];
      if (!lastItem.key) {
        // don't add, the last key is required
        return;
      }
    }
    const maxId = maxValue(propertyList.map((_) => _.id));
    propertyList.push({ value: "", key: "", id: maxId + 1, ...customValue });
    setPropertyList([...propertyList]);
  };

  const getDescription = (key) => {
    const el = suggestions?.find((el) => key === el.key);
    return el?.description;
  };

  React.useEffect(() => {
    if (propertyList) {
      setProperties(listToDict(propertyList));
    }
  }, [propertyList]);

  // updates the parent on changes
  React.useEffect(() => {
    if (properties) {
      onPropertiesChanged(properties);
    }
  }, [properties, onPropertiesChanged]);

  return (
    <React.Fragment>
      <Typography className="bold my-4">
        {label ?? "Property Editor"}
      </Typography>

      {suggestions ? (
        <SuggestedProperties
          suggestions={suggestions}
          currentProperties={propertyList}
          addProperty={handleNewEntry}
        />
      ) : null}

      {propertyList.length === 0 ? (
        <EmptyList>
          <div>
            <div>{placeholder}</div>
            <button
              onClick={handleNewEntry}
              className="btn btn-outline-success mt-3 px-4"
            >
              Add Property
            </button>
          </div>
        </EmptyList>
      ) : null}

      {propertyList.length > 0 ? (
        <div className="row mb-3">
          <div className="col">
            <Typography variant="label" className="semi-bold">
              Property
            </Typography>
          </div>
          <div className="col">
            <Typography variant="label" className="semi-bold">
              Value
            </Typography>
          </div>
          <div className="col-1" />
        </div>
      ) : null}

      {propertyList.map((p, i) => (
        <PropertyRow
          key={p.id}
          id={p.id}
          entry={p.key}
          value={p.value}
          isLast={i === propertyList.length - 1}
          hint={getDescription(p.key)}
          onChange={handleChange}
          onRemove={handleRemove}
          onAdd={handleNewEntry}
        />
      ))}
    </React.Fragment>
  );
};
