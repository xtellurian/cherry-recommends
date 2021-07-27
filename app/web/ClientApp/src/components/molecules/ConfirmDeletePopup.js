import React from "react";
import { ConfirmationPopup } from "./ConfirmationPopup";
import { ErrorCard } from "./ErrorCard";

export const ConfirmDeletePopup = ({
  entity,
  open,
  setOpen,
  error,
  handleDelete,
}) => {
  return (
    <ConfirmationPopup
      isOpen={open}
      setIsOpen={setOpen}
      label="Do you really want to delete this?"
    >
      <div className="m-2">{entity.name || entity.commonId}</div>
      {error && <ErrorCard error={error} />}
      <div
        className="btn-group"
        role="group"
        aria-label="Delete or cancel buttons"
      >
        <button className="btn btn-secondary" onClick={() => setOpen(false)}>
          Cancel
        </button>
        <button className="btn btn-danger" onClick={handleDelete}>
          Delete
        </button>
      </div>
    </ConfirmationPopup>
  );
};
