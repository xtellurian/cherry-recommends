import React from "react";
import { Broadcast } from "react-bootstrap-icons";
import { useNavigation } from "../../utility/useNavigation";
import FlexRow from "../molecules/layout/EntityFlexRow";

export const ChannelRow = ({ channel, children }) => {
  const { navigate } = useNavigation();

  const handleClick = () => {
    navigate({ pathname: `/channels` }); // TODO: navigate to details
  };

  return (
    <FlexRow
      className="clickable-row"
      onClick={handleClick}
      style={{ cursor: "pointer" }}
    >
      <div className="flex-shrink-0">
        <Broadcast className="m-2" size={25} />
      </div>
      <div className="flex-grow-1">{channel.name}</div>
      {children ? (
        <div onClick={(e) => e.stopPropagation()}>{children}</div>
      ) : null}
    </FlexRow>
  );
};
