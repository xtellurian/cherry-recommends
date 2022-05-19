import React from "react";
import { CaretRightFill } from "react-bootstrap-icons";
import { DateTimeField } from "../molecules/DateTimeField";
import FlexRow from "../molecules/layout/EntityFlexRow";
import { BigPopup } from "../molecules/popups/BigPopup";
import { RecommendationDetail } from "./RecommendationDetail";

export const RecommendationRow = ({ recommendation }) => {
  const [isPopupOpen, setIsPopupOpen] = React.useState(false);

  return (
    <React.Fragment>
      <BigPopup isOpen={isPopupOpen} setIsOpen={setIsPopupOpen}>
        <RecommendationDetail recommendationId={recommendation.id} />
      </BigPopup>
      <FlexRow
        className="clickable-row"
        onClick={() => setIsPopupOpen(true)}
        style={{ cursor: "pointer" }}
      >
        <div>
          <CaretRightFill style={{ color: "var(--cherry-pink)" }} />
          <span className="ml-2">{recommendation.recommenderType}</span>
        </div>
        <DateTimeField date={recommendation.created} />
      </FlexRow>
    </React.Fragment>
  );
};
