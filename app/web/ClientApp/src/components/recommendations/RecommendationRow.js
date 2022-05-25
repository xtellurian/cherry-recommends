import React from "react";
import { CaretRightFill } from "react-bootstrap-icons";
import { DateTimeField } from "../molecules/DateTimeField";
import FlexRow from "../molecules/layout/EntityFlexRow";
import { BigPopup } from "../molecules/popups/BigPopup";
import { RecommendationDetail } from "./RecommendationDetail";

const typeNameMap = {
  product: "Product",
  parameterSet: "Parameter",
  items: "Promotion",
};

export const RecommendationRow = ({ recommendation }) => {
  const [isPopupOpen, setIsPopupOpen] = React.useState(false);
  const modelInput = recommendation.modelInput
    ? JSON.parse(recommendation.modelInput)
    : {};
  const customerId = modelInput?.customerId;
  const businessId = modelInput?.businessId;
  const modelOutput = recommendation.modelOutput
    ? JSON.parse(recommendation.modelOutput)
    : {};
  const topPromo = modelOutput?.scoredItems?.find(
    (e) => typeof e !== "undefined"
  )?.item;
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
          {topPromo ? (
            <span className="ml-2">{topPromo.name}</span>
          ) : (
            <span className="ml-2">
              {typeNameMap[recommendation.recommenderType]}
            </span>
          )}
          {customerId ? <span className="ml-2">for {customerId}</span> : null}
          {businessId ? <span className="ml-2">for {businessId}</span> : null}
        </div>
        <DateTimeField date={recommendation.created} />
      </FlexRow>
    </React.Fragment>
  );
};
