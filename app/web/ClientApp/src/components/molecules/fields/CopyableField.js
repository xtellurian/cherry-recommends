import Tippy from "@tippyjs/react";
import React from "react";
import { EditPropertyPopup } from "../popups/EditPropertyPopup";

export const CopyableField = ({
  label,
  value,
  isSecret,
  isNumeric,
  min,
  max,
  isEditable,
  onValueEdited,
  tooltip,
}) => {
  const [editing, setEditing] = React.useState(false);

  const toggleEdit = () => {
    setEditing(!editing);
  };

  const editType = isSecret ? "password" : isNumeric ? "number" : "text";
  const displayType = isSecret ? "password" : "text";

  if (isEditable && onValueEdited === undefined) {
    throw new Error("onValueEdited must be a function");
  }

  return (
    <React.Fragment>
      <EditPropertyPopup
        setIsOpen={setEditing}
        label={label}
        value={value}
        type={editType}
        min={min}
        max={max}
        isOpen={editing}
        onSetValue={(v) => onValueEdited(v)}
      />
      <Tippy
        content={
          tooltip && (
            <div className="bg-light text-center border border-primary rounded p-1">
              {tooltip}
            </div>
          )
        }
      >
        <div className="input-group mb-3">
          <div className="input-group-prepend ml-1">
            <span className="input-group-text">{label}</span>
          </div>
          <input
            type={displayType}
            value={value || ""} // dont allow uncontrolled
            className="form-control"
            aria-label={label}
            disabled
          />

          {isEditable && (
            <button
              className="btn btn-outline-secondary"
              type="button"
              onClick={toggleEdit}
            >
              Edit
            </button>
          )}

          <button
            className="btn btn-outline-secondary"
            type="button"
            onClick={() => navigator.clipboard.writeText(value)}
          >
            Copy
          </button>
        </div>
      </Tippy>
    </React.Fragment>
  );
};
