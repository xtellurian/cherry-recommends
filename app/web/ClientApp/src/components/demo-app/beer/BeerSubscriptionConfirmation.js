import React from "react";
import { useLocation } from "react-router-dom";

import { trackOfferAccepted } from "../../../api/eventsApi";

function useQuery() {
  return new URLSearchParams(useLocation().search);
}

export const BeerSubscriptionConfirmation = () => {
  console.log('loading confirm conponent')
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
