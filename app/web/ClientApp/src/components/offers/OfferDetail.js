import React from "react";
import { useParams } from "react-router-dom";
import { Spinner } from "../molecules/Spinner";
import { Title, Subtitle } from "../molecules/PageHeadings";
import { useOffer } from "../../api-hooks/offersApi";

export const OfferDetail = () => {
  let { id } = useParams();
  const offer = useOffer({ id });
  return (
    <React.Fragment>
      <div>
        <Title>Offer</Title>
        <Subtitle>{offer?.name || offer.id}</Subtitle>
        <hr />
        {offer.loading && <Spinner>Loading Offer</Spinner>}
        {offer && (
          <div className="card">
            <div className="card-body">
              <div>Price: {offer.price}</div>
              <div>Cost: {offer.cost}</div>
              <div>Currency: {offer.currency}</div>
            </div>
          </div>
        )}
      </div>
    </React.Fragment>
  );
};
