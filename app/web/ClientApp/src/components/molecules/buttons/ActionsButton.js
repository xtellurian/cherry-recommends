import React from "react";
import {
  ButtonDropdown,
  DropdownMenu,
  DropdownToggle,
  DropdownItem,
} from "reactstrap";

import { Navigation } from "../Navigation";

// dont set to and onClick at the same time
export const ActionsButton = ({ to, onClick, label, className, children }) => {
  if (to && onClick) {
    throw new Error("Dont set to and onClick at the same time");
  }
  const [isOpen, setIsOpen] = React.useState(false);
  const toggle = () => {
    setIsOpen(!isOpen);
  };

  return (
    <span className={className || "float-right btn-group"}>
      {to ? (
        <Navigation to={to}>
          <button className="btn btn-primary mr-1">{label}</button>
        </Navigation>
      ) : null}
      {onClick ? (
        <button onClick={onClick} className="btn btn-primary mr-1">
          {label}
        </button>
      ) : null}
      {children && (
        <ButtonDropdown isOpen={isOpen} toggle={toggle}>
          <DropdownToggle className="pl-2 pr-2" split color="primary" />
          {children}
        </ButtonDropdown>
      )}
    </span>
  );
};

export const ActionItemsGroup = ({ label, children }) => {
  return (
    <DropdownMenu right>
      {children && <DropdownItem header>{label}</DropdownItem>}
      {children}
    </DropdownMenu>
  );
};

export const ActionLink = ({ to, children }) => {
  return (
    <Navigation to={to}>
      <DropdownItem>{children}</DropdownItem>
    </Navigation>
  );
};

export const ActionItem = ({ onClick, children }) => {
  return (
    <div onClick={onClick}>
      <DropdownItem>{children}</DropdownItem>
    </div>
  );
};
