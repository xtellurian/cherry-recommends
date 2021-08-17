import React from "react";
import Modal from "react-modal";

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

export const ConfirmationPopup = ({ isOpen, setIsOpen, label, children }) => {
  const onRequestClose = () => setIsOpen(false);
  return (
    <Modal
      isOpen={isOpen}
      onRequestClose={onRequestClose}
      style={customStyles}
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
