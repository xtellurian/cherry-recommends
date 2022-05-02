import React from "react";
import Modal from "react-modal";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";
import { big, closeButton } from "./styles";

export const BigPopup = (props) => {
  const onRequestClose = () => props.setIsOpen(false);
  return (
    <Modal {...props} onRequestClose={onRequestClose} style={big}>
      <button
        className="btn btn-link"
        onClick={onRequestClose}
        style={closeButton}
      >
        <FontAwesomeIcon icon={faCircleXmark} />
      </button>
      {props.children}
    </Modal>
  );
};
