import React from "react";
import { People } from "react-bootstrap-icons";
import { useHistory } from "react-router-dom";
import FlexRow from "../molecules/layout/EntityFlexRow";

export const BusinessRow = ({ business, children }) => {
  const history = useHistory();
  const handleClick = () => history.push(`/businesses`);

  return (
    <FlexRow
      className="clickable-row"
      onClick={handleClick}
      style={{ cursor: "pointer" }}
    >
      <div className="flex-shrink-0">
        <People className="m-2" size={25} />
      </div>
      <div className="flex-grow-1">{business.name || business.commonId}</div>
      {children ? (
        <div onClick={(e) => e.stopPropagation()}>{children}</div>
      ) : null}
    </FlexRow>
  );
};
