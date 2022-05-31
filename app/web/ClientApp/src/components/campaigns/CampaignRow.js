import React from "react";
import { Megaphone } from "react-bootstrap-icons";
import { useNavigation } from "../../utility/useNavigation";

import FlexRow from "../molecules/layout/EntityFlexRow";

export const CampaignRow = ({ recommender, children }) => {
  const { navigate } = useNavigation();

  const subPath = recommender.baselineItemId
    ? "promotions-campaigns"
    : "parameter-set-campaigns";

  const handleClick = () => {
    navigate({
      pathname: `/campaigns/${subPath}/detail/${recommender.id}`,
    });
  };

  return (
    <FlexRow
      className="clickable-row"
      onClick={handleClick}
      style={{ cursor: "pointer" }}
    >
      <div className="flex-shrink-0">
        <Megaphone className="m-2" size={25} />
      </div>
      <div className="flex-grow-1 text-left">{recommender.name}</div>
      {children ? (
        <div onClick={(e) => e.stopPropagation()}>{children}</div>
      ) : null}
    </FlexRow>
  );
};
