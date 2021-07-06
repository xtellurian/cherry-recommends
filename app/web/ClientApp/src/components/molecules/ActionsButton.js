import React from "react";
import { Link } from "react-router-dom";
import {
  ButtonDropdown,
  DropdownMenu,
  DropdownToggle,
  DropdownItem,
} from "reactstrap";

export const ActionsButton = ({ to, label, className, children }) => {
  const [isOpen, setIsOpen] = React.useState(false);
  const toggle = () => {
    setIsOpen(!isOpen);
  };

  return (
    <span className={className || "float-right"}>
      <ButtonDropdown isOpen={isOpen} toggle={toggle}>
        <Link to={to}>
          <button className="btn btn-primary float-right">{label}</button>
        </Link>
        {children && <DropdownToggle split color="primary" />}
        {children}
      </ButtonDropdown>
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
    <Link to={to}>
      <DropdownItem>{children}</DropdownItem>
    </Link>
  );
};

export const ActionItem = ({ onClick, children }) => {
  return (
    <div onClick={onClick}>
      <DropdownItem>{children}</DropdownItem>
    </div>
  );
};
