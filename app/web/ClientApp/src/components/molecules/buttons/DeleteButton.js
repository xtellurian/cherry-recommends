import React from "react";
import { Trash } from "react-bootstrap-icons";

export const DeleteButton = ({ children, className, ...other }) => {
  return (
    <button {...other} className={`btn btn-danger ${className || ""}`}>
      {(children && children) || <Trash />}
    </button>
  );
};
