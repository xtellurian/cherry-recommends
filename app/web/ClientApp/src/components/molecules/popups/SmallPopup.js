import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React from "react";
import Modal from "react-modal";
import { closeButton, small } from "./styles";

export const SmallPopup = (props) => {
  const onRequestClose = () => props.setIsOpen(false);
  return (
    <Modal {...props} onRequestClose={onRequestClose} style={small}>
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
