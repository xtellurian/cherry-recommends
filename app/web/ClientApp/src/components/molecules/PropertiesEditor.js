import React from "react";
import { Subtitle } from "./layout";
import { EmptyList } from "./empty/EmptyList";

const PropertyRow = ({ id, entry, value, onChange, onRemove }) => {
  const [state, setState] = React.useState({
    key: entry,
    value,
  });

  React.useEffect(() => {
    if (state.key !== entry || state.value !== value) {
      setState({ key: entry, prevKey: state.prevKey, value: value });
    }
  }, [entry, value]);

  const handleUpdate = () => {
    onChange(id, state.key, state.value);
  };

  return (
    <div className="row mt-1">
      <div className="col">
        <div className="input-group">
          <div className="input-group-prepend ml-1">
            <span className="input-group-text" id="basic-addon3">
              Property
            </span>
          </div>
          <input
            type="text"
            className="form-control"
            placeholder="Entry"
            value={state.key}
            onBlur={handleUpdate}
            onChange={(e) =>
              setState({
                key: e.target.value,
                value: state.value,
              })
            }
          />
          {/* <div className="ml-l input-group-prepend ml-1">
            <span className="input-group-text" id="basic-addon3">
              Value:
            </span>
          </div> */}
          <input
            type="text"
            className="form-control"
            placeholder="Value"
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
      </div>
      <div className="col-1">
        <button
          onClick={() => onRemove(id)}
          className="btn btn-outline-danger btn-block"
        >
          X
        </button>
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
}) => {
  const [properties, setProperties] = React.useState(initialProperties || {});
  const [propertyList, setPropertyList] = React.useState(
    dictToList(initialProperties || {})
  );
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

  const handleChange = (id, key, value) => {
    const prop = propertyList.find((_) => _.id === id);
    if (prop) {
      prop.key = key;
      prop.value = value;
      setPropertyList([...propertyList]);
    } else {
      console.log("warning: couldn't find property ID");
    }
  };
  const handleRemove = (id) => {
    const propIndex = propertyList.findIndex((_) => _.id === id);
    if (propIndex > -1) {
      propertyList.splice(propIndex, 1);
      setPropertyList([...propertyList]);
    }
  };

  const handleNewEntry = () => {
    // check if the last one is empty, and don't add if so
    if (propertyList && propertyList.length > 0) {
      const lastItem = propertyList[propertyList.length - 1];
      if (!lastItem.key) {
        // don't add, the last key is required
        return;
      }
    }
    const maxId = maxValue(propertyList.map((_) => _.id));
    propertyList.push({ value: "", key: "", id: maxId + 1 });
    setPropertyList([...propertyList]);
  };

  return (
    <React.Fragment>
      <Subtitle> {label || "Property Editor"}</Subtitle>
      {propertyList.length === 0 && (
        <EmptyList>
          <div>{placeholder}</div>
        </EmptyList>
      )}
      {propertyList.map((p, i) => (
        <PropertyRow
          key={p.id}
          id={p.id}
          entry={p.key}
          value={p.value}
          onChange={handleChange}
          onRemove={handleRemove}
        />
      ))}
      <div className="row m-2 justify-content-center">
        <div className="col-4 text-center">
          <button
            onClick={handleNewEntry}
            className="btn btn-outline-success btn-block"
          >
            Add Property
          </button>
        </div>
      </div>
    </React.Fragment>
  );
};
