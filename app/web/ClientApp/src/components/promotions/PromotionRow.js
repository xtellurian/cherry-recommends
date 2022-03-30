import React from "react";
import { Tag } from "react-bootstrap-icons";

import { useNavigation } from "../../utility/useNavigation";
import FlexRow from "../molecules/layout/EntityFlexRow";

export const PromotionRow = ({ promotion, children }) => {
  const { navigate } = useNavigation();

  const handleClick = () => {
    navigate({ pathname: `/promotions/detail/${promotion.id}` });
  };

  const shortenDescription = (description) => {
    if (!description || description === "") {
      return "No Description";
    } else if (description.length > 22) {
      return description.substring(0, 22) + "...";
    } else {
      return description;
    }
  };

  return (
    <FlexRow
      className="clickable-row"
      onClick={handleClick}
      style={{ cursor: "pointer" }}
    >
      <div className="flex-shrink-1">
        <Tag className="m-2" size={25} />
      </div>{" "}
      <div className="flex-grow-1 text-left">{promotion.name}</div>
      <div>{shortenDescription(promotion.description)}</div>
      <div className="flex-shrink-1 p-2">{children}</div>
    </FlexRow>
  );
};
