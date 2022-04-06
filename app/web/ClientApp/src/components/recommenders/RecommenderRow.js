import React from "react";
import { EmojiHeartEyes } from "react-bootstrap-icons";
import { useNavigation } from "../../utility/useNavigation";

import FlexRow from "../molecules/layout/EntityFlexRow";

export const RecommenderRow = ({ recommender, children }) => {
  const { navigate } = useNavigation();

  const subPath = recommender.baselineItemId
    ? "promotions-recommenders"
    : "parameter-set-recommenders";

  const handleClick = () => {
    navigate({
      pathname: `/recommenders/${subPath}/detail/${recommender.id}`,
    });
  };

  return (
    <FlexRow
      className="clickable-row"
      onClick={handleClick}
      style={{ cursor: "pointer" }}
    >
      <div className="flex-shrink-0">
        <EmojiHeartEyes className="m-2" size={25} />
      </div>
      <div className="flex-grow-1 text-left">{recommender.name}</div>
      {children ? (
        <div onClick={(e) => e.stopPropagation()}>{children}</div>
      ) : null}
    </FlexRow>
  );
};
