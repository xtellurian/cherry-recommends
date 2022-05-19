import React from "react";
import { CaretRightFill } from "react-bootstrap-icons";
import { DateTimeField } from "../molecules/DateTimeField";
import FlexRow from "../molecules/layout/EntityFlexRow";
import { BigPopup } from "../molecules/popups/BigPopup";
import { RecommendationDetail } from "./RecommendationDetail";

export const OfferRow = ({ offer }) => {
  const [isPopupOpen, setIsPopupOpen] = React.useState(false);

  return (
    <React.Fragment>
      <BigPopup isOpen={isPopupOpen} setIsOpen={setIsPopupOpen}>
        <RecommendationDetail recommendationId={offer.recommendationId} />
      </BigPopup>
      <FlexRow
        className="clickable-row"
        style={{ cursor: "pointer" }}
        onClick={() => setIsPopupOpen(true)}
      >
        <div>
          <CaretRightFill style={{ color: "var(--cherry-pink)" }} />
          <span className="ml-2">{offer.state}</span>
        </div>
        <DateTimeField date={offer.created} />
      </FlexRow>
    </React.Fragment>
  );
};
