import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React from "react";
import Modal from "react-modal";
import { closeButton, small } from "./styles";

export const ConfirmationPopup = ({ isOpen, setIsOpen, label, children }) => {
  const onRequestClose = () => setIsOpen(false);
  return (
    <Modal
      isOpen={isOpen}
      onRequestClose={onRequestClose}
      style={small}
      contentLabel="Modal Confirmation"
    >
      <button
        className="btn btn-link"
        onClick={onRequestClose}
        style={closeButton}
      >
        <FontAwesomeIcon icon={faCircleXmark} />
      </button>
      <div className="text-center">
        <div className="pb-3">
          <h5>{label}</h5>
        </div>
        <div className="m-2">{children}</div>
      </div>
    </Modal>
  );
};
