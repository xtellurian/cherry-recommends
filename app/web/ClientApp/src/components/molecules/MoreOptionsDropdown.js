import React from "react";
import { Link } from "react-router-dom";
import {
  Dropdown,
  ButtonDropdown,
  DropdownMenu,
  DropdownToggle,
  DropdownItem,
} from "reactstrap";

export const MoreOptionsDropdown = ({ to, label, className, children }) => {
  const [isOpen, setIsOpen] = React.useState(false);
  const toggle = () => {
    setIsOpen(!isOpen);
  };

  return (
    <span className={className || "float-right"}>
      <Dropdown isOpen={isOpen} toggle={toggle}>
        <DropdownToggle className="btn btn-secondary" caret>
          {label || "Options"}
        </DropdownToggle>
        {children}
      </Dropdown>
    </span>
  );
};

export const OptionItemsGroup = ({ label, children }) => {
  return (
    <DropdownMenu right>
      {children && <DropdownItem header>{label}</DropdownItem>}
      {children}
    </DropdownMenu>
  );
};

export const OptionLink = ({ to, children }) => {
  return (
    <Link to={to}>
      <DropdownItem>{children}</DropdownItem>
    </Link>
  );
};

export const OptionItem = ({ onClick, children }) => {
  return (
    <div onClick={onClick}>
      <DropdownItem>{children}</DropdownItem>
    </div>
  );
};
