import React from "react";
import Modal from "react-modal";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";
import { big, closeButton } from "./styles";
import { Typography } from "../Typography";

export const Heading = ({ children }) => {
  return (
    <React.Fragment>
      <Typography variant="h5">{children}</Typography>
    </React.Fragment>
  );
};
const pinBottom = {
  position: "absolute",
  bottom: 40,
  left: 0,
  width: "100%",
  height: "50px",
  // paddingLeft: "20px",
  // paddingRight: "20px",
  padding: "20px",
};
export const BigPopup = ({
  header,
  headerDivider,
  buttons,
  children,
  ...props
}) => {
  const onRequestClose = () => props.setIsOpen(false);
  const HeadingWrapper = () => {
    if (typeof header === "object") {
      return header;
    } else if (typeof header === "string") {
      return <Heading>{header}</Heading>;
    } else {
      return null;
    }
  };
  return (
    <Modal {...props} onRequestClose={onRequestClose} style={big}>
      <button
        className="btn btn-link"
        onClick={onRequestClose}
        style={closeButton}
      >
        <FontAwesomeIcon icon={faCircleXmark} />
      </button>
      <div style={{ minHeight: "25px" }}>
        <HeadingWrapper />
        {headerDivider ? <hr className="mb-4" /> : null}
      </div>
      <div style={{ minHeight: "200px", paddingBottom: "45px" }}>
        {children}
      </div>

      {buttons ? <div style={pinBottom}>{buttons}</div> : null}
    </Modal>
  );
};
