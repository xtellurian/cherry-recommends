import React from "react";
import { Person } from "react-bootstrap-icons";
import { useHistory } from "react-router-dom";
import FlexRow from "../molecules/layout/EntityFlexRow";

export const CustomerRow = ({ customer, children }) => {
  const history = useHistory();
  const handleClick = () => history.push(`/customers/detail/${customer.id}`);

  return (
    <FlexRow
      className="clickable-row"
      onClick={handleClick}
      style={{ cursor: "pointer" }}
    >
      <div className="flex-shrink-0">
        <Person className="m-2" size={25} />
      </div>
      <div className="flex-grow-1">{customer.name || customer.customerId}</div>
      {children ? (
        <div onClick={(e) => e.stopPropagation()}>{children}</div>
      ) : null}
    </FlexRow>
  );
};
