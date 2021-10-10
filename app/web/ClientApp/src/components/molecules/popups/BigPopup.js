import React from "react";
import Modal from "react-modal";
import { big } from "./styles";

export const BigPopup = (props) => {
  const onRequestClose = () => props.setIsOpen(false);
  return (
    <Modal {...props} onRequestClose={onRequestClose} style={big}>
      {props.children}
    </Modal>
  );
};
