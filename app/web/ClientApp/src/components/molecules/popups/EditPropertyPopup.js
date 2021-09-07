import React from "react";
import Modal from "react-modal";
import { InputGroup, TextInput } from "../TextInput";
import { small } from "./styles";

export const EditPropertyPopup = ({
  isOpen,
  setIsOpen,
  label,
  value,
  onSetValue,
}) => {
  const onRequestClose = () => setIsOpen(false);
  const [newValue, setNewValue] = React.useState(value);

  if (onSetValue === undefined) {
    throw new Error("onSetValue is not a function");
  }
  const handleSetValue = () => {
    onSetValue(newValue);
    setIsOpen(false);
  };
  return (
    <Modal
      isOpen={isOpen}
      onRequestClose={onRequestClose}
      style={small}
      contentLabel="Edit Property"
    >
      <div className="text-center">
        <div className="m-2">
          <InputGroup>
            <TextInput
              label={label}
              value={newValue}
              onChange={(v) => setNewValue(v.target.value)}
            />
          </InputGroup>
          <div
            className="btn-group mt-1 w-100"
            role="group"
            aria-label="Delete or rename buttons"
          >
            <button
              className="btn btn-secondary"
              onClick={() => setIsOpen(false)}
            >
              Cancel
            </button>
            <button className="btn btn-success" onClick={handleSetValue}>
              Confirm
            </button>
          </div>
        </div>
      </div>
    </Modal>
  );
};