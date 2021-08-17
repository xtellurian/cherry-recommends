import React from "react";
import Modal from "react-modal";
import { InputGroup, TextInput } from "../TextInput";
const customStyles = {
  content: {
    top: "50%",
    left: "50%",
    right: "auto",
    bottom: "auto",
    marginRight: "-50%",
    transform: "translate(-50%, -50%)",
  },
};

export const RenameEntityPopup = ({
  isOpen,
  setIsOpen,
  label,
  entity,
  onRename,
}) => {
  const onRequestClose = () => setIsOpen(false);
  const [newName, setNewName] = React.useState(entity.name);

  if (onRename === undefined) {
    throw  "onRename is not a function";
  }
  const handleRename = () => {
    onRename(newName);
    setIsOpen(false);
  };
  return (
    <Modal
      isOpen={isOpen}
      onRequestClose={onRequestClose}
      style={customStyles}
      contentLabel="Rename"
    >
      <div className="text-center">
        <div className="pb-3">
          <h5>{label}</h5>
        </div>
        <div className="m-2">
          <InputGroup>
            <TextInput
              label="Name"
              value={newName}
              onChange={(v) => setNewName(v.target.value)}
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
            <button className="btn btn-success" onClick={handleRename}>
              Confirm
            </button>
          </div>
        </div>
      </div>
    </Modal>
  );
};
