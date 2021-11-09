import React from "react";
import Modal from "react-modal";
import { small } from "./styles";

export const SmallPopup = (props) => {
  const onRequestClose = () => props.setIsOpen(false);
  return (
    <Modal {...props} onRequestClose={onRequestClose} style={small}>
      {props.children}
    </Modal>
  );
};
