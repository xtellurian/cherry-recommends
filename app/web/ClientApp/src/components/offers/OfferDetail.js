import React from "react";
import { useParams } from "react-router-dom";
import { Spinner } from "../molecules/Spinner";
import { Title } from "../molecules/PageHeadings";
import { fetchOffer } from "../../api/offersApi";

export const OfferDetail = () => {
  let { id } = useParams();
  const [offer, setOffer] = React.useState();
  React.useEffect(() => {
    fetchOffer({
      success: setOffer,
      error: () => alert("Error fetching offer"),
      id,
    });
  }, [id]);
  return (
    <React.Fragment>
      <div>
        <Title>Offer | {offer && offer.name}</Title>
        <div>{id}</div>
        <hr />
        {!offer && <Spinner />}
        {offer && (
          <div>
            <div>
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
