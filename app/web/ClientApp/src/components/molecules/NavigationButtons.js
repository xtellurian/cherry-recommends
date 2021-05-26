import React from "react";
import { useHistory } from "react-router-dom";
import { ArrowLeft } from "react-bootstrap-icons";
export const BackArrow = ({ to }) => {
  const { goBack } = useHistory();
  return (
    <React.Fragment>
      <ArrowLeft
        color="royalblue"
        className="mr-1"
        role="button"
        style={{ cursor: "pointer" }}
        onClick={goBack}
      />
    </React.Fragment>
  );
};
