import React from "react";
// import { trackOfferAccepted } from "../../../api/eventsApi";
import { useQuery } from "../../../utility/utility";

const trackOfferAccepted = () => {
  alert('not implemented')
}
export const BeerSubscriptionConfirmation = () => {
  const query = useQuery();
  let offerId = query.get("offerId") || "";

  React.useEffect(() => {
    trackOfferAccepted({
      success: () => alert("Offer acceptance recorded"),
      error: console.log,
      offerId,
    });
  });
  return (
    <React.Fragment>
      <div className="text-center mt-4">
        <h3>Your subscription is confirmed</h3>
      </div>
    </React.Fragment>
  );
};
