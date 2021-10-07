import React from "react";
import { EditPropertyPopup } from "../popups/EditPropertyPopup";

export const CopyableField = ({
  label,
  value,
  isSecret,
  isEditable,
  onValueEdited,
}) => {
  const [editing, setEditing] = React.useState(false);

  const toggleEdit = () => {
    setEditing(!editing);
  };

  if (isEditable && onValueEdited === undefined) {
    throw new Error("onValueEdited must be a function");
  }

  return (
    <React.Fragment>
      <EditPropertyPopup
        setIsOpen={setEditing}
        label={label}
        value={value}
        isOpen={editing}
        onSetValue={(v) => onValueEdited(v)}
      />
      <div className="input-group mb-3">
        <div className="input-group-prepend ml-1">
          <span className="input-group-text">{label}</span>
        </div>
        <input
          type={isSecret ? "password" : "text"}
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
    </React.Fragment>
  );
};
