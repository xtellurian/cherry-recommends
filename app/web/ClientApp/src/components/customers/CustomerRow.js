import React from "react";
import { Person } from "react-bootstrap-icons";

import { useNavigation } from "../../utility/useNavigation";
import FlexRow from "../molecules/layout/EntityFlexRow";
import { DateTimeField } from "../molecules/DateTimeField";

export const CustomerRow = ({ customer, children }) => {
  const { navigate } = useNavigation();

  const handleClick = () => {
    navigate({ pathname: `/customers/customers/detail/${customer.id}` });
  };

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
      <DateTimeField date={customer.lastUpdated} />
      {children ? (
        <div onClick={(e) => e.stopPropagation()}>{children}</div>
      ) : null}
    </FlexRow>
  );
};
