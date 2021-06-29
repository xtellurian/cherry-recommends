import React from "react";
import { Link, useRouteMatch } from "react-router-dom";
import { useOffers } from "../../api-hooks/offersApi";
import { Title } from "../molecules/PageHeadings";
import { ExpandableCard } from "../molecules/ExpandableCard";
import { Spinner } from "../molecules/Spinner";
import { CreateButton } from "../molecules/CreateButton";
import { Paginator } from "../molecules/Paginator";

const OfferRow = ({ offer }) => {
  return (
    <ExpandableCard label={offer.name}>
      <Link to={`/offers/${offer.id}`} className="float-right">
        <button className="btn btn-secondary">View</button>
      </Link>
      <div>
        <div>Price: {offer.price}</div>
        <div>Cost: {offer.cost}</div>
        <div>Currency: {offer.currency}</div>
      </div>
    </ExpandableCard>
  );
};
export const OffersSummary = () => {
  let { path } = useRouteMatch();
  const result = useOffers();
  return (
    <React.Fragment>
      <div>
        <CreateButton to="/offers/create" className="float-right">
          Create New Offer
        </CreateButton>

        <Title>Offers</Title>
      </div>
      <hr />
      <div>
        {result && result.items &&
          result.items.length > 0 &&
          result.items.map((o) => <OfferRow key={o.id} offer={o} />)}
      </div>
      <div>
        {result && result.items && result.items.length === 0 && (
          <div className="text-center p-5">
            <div>There are no offers.</div>
            <CreateButton to={`${path}/create`} className="mt-4">
              Create New Offer
            </CreateButton>
          </div>
        )}
      </div>
      {!result && <Spinner />}
      {result && <Paginator {...result.pagination} />}
    </React.Fragment>
  );
};
