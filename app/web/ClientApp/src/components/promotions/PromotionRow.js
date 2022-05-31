import React from "react";
import { Tag } from "react-bootstrap-icons";

import { useNavigation } from "../../utility/useNavigation";
import FlexRow from "../molecules/layout/EntityFlexRow";

export const PromotionRow = ({ promotion, children }) => {
  const { navigate } = useNavigation();

  const handleClick = () => {
    navigate({ pathname: `/promotions/promotions/detail/${promotion.id}` });
  };

  const shortenDescription = (description) => {
    if (!description || description === "") {
      return "";
    } else if (description.length > 22) {
      return description.substring(0, 22) + "...";
    } else {
      return description;
    }
  };

  const stopPropagation = (e) => e.stopPropagation();

  return (
    <FlexRow
      className="clickable-row"
      style={{ cursor: "pointer" }}
      onClick={handleClick}
    >
      <div className="flex-shrink-1">
        <Tag className="m-2" size={25} />
      </div>{" "}
      <div className="flex-grow-1 text-left">{promotion.name}</div>
      <div>{shortenDescription(promotion.description)}</div>
      {children ? (
        <div className="flex-shrink-1 p-2" onClick={stopPropagation}>
          {children}
        </div>
      ) : null}
    </FlexRow>
  );
};
