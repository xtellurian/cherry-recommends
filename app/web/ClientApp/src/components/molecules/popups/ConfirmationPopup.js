import React from "react";
import Modal from "react-modal";
import { small } from "./styles";

export const ConfirmationPopup = ({ isOpen, setIsOpen, label, children }) => {
  const onRequestClose = () => setIsOpen(false);
  return (
    <Modal
      isOpen={isOpen}
      onRequestClose={onRequestClose}
      style={small}
      contentLabel="Modal Confirmation"
    >
      <div className="text-center">
        <div className="pb-3">
          <h5>{label}</h5>
        </div>
        <div className="m-2">{children}</div>
      </div>
    </Modal>
  );
};
