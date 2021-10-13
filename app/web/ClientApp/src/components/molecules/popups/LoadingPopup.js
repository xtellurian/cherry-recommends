import React from "react";
import Modal from "react-modal";
import { Spinner } from "../Spinner";
import { small } from "./styles";

export const LoadingPopup = ({ loading, label }) => {
  return (
    <Modal
      isOpen={loading}
      //   onRequestClose={onRequestClose}
      style={small}
      contentLabel="Loading..."
    >
      <div className="text-center">
        <div className="m-2">
          <Spinner> {label}</Spinner>
        </div>
      </div>
    </Modal>
  );
};
