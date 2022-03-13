import React from "react";
import { People } from "react-bootstrap-icons";
import { useHistory } from "react-router-dom";
import FlexRow from "../molecules/layout/EntityFlexRow";

export const SegmentRow = ({ segment, children }) => {
  const history = useHistory();
  const handleClick = () => history.push(`/segments/detail/${segment.id}`);

  return (
    <FlexRow
      className="clickable-row"
      onClick={handleClick}
      style={{ cursor: "pointer" }}
    >
      <div className="flex-shrink-0">
        <People className="m-2" size={25} />
      </div>
      <div className="flex-grow-1">{segment?.name}</div>
      {children ? (
        <div onClick={(e) => e.stopPropagation()}>{children}</div>
      ) : null}
    </FlexRow>
  );
};
