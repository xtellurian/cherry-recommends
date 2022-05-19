import React from "react";
import { useParams } from "react-router-dom";
import { Spinner, ErrorCard, EmptyList, Paginator } from "../../molecules";
import { useOffers } from "../../../api-hooks/promotionsRecommendersApi";
import { OfferRow } from "../../recommendations/OfferRow";

// Component is currently not used. Leaving it here in case we want to display a list of offers
export const OfferList = ({ size }) => {
  const { id } = useParams();
  const offers = useOffers({ id, pageSize: 5 });

  return (
    <React.Fragment>
      {offers.loading && <Spinner />}
      {offers.error && <ErrorCard error={offers.error} />}
      {offers.items && offers.items.length === 0 && (
        <EmptyList>This recommender is yet to make a recommendation.</EmptyList>
      )}
      {offers.items &&
        offers.items.map((r) => <OfferRow offer={r} key={r.id} />)}
      {offers.pagination && size !== "sm" && (
        <Paginator {...offers.pagination} />
      )}
    </React.Fragment>
  );
};
