import React from "react";
import { EmojiHeartEyes } from "react-bootstrap-icons";
import { useHistory } from "react-router-dom";
import FlexRow from "../molecules/layout/EntityFlexRow";

export const RecommenderRow = ({ recommender, children }) => {
  const subPath = recommender.baselineItemId
    ? "promotions-recommenders"
    : "parameter-set-recommenders";
  const recommenderType = recommender.baselineItemId ? "Promos" : "Parameters";

  const history = useHistory();
  const handleClick = () =>
    history.push(`/recommenders/${subPath}/detail/${recommender.id}`);

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
      <div className="mr-2">{recommenderType}</div>
      {children ? (
        <div onClick={(e) => e.stopPropagation()}>{children}</div>
      ) : null}
    </FlexRow>
  );
};
