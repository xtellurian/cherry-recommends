import React from "react";
import { EmojiHeartEyes } from "react-bootstrap-icons";
import { useHistory } from "react-router-dom";

import FlexRow from "../molecules/layout/EntityFlexRow";
import { tabs } from "./promotions-recommenders/ItemsRecommenderPrimaryNav";

export const RecommenderRow = ({ recommender, children }) => {
  const history = useHistory();
  const queryParams = new URLSearchParams(history.location.search);
  queryParams.set("tab", tabs[0].id);

  const subPath = recommender.baselineItemId
    ? "promotions-recommenders"
    : "parameter-set-recommenders";

  const handleClick = () => {
    history.push({
      ...history.location,
      pathname: `/recommenders/${subPath}/detail/${recommender.id}`,
      search: queryParams.toString(),
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
