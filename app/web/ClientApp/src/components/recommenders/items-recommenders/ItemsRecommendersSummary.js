import React from "react";
import { Link } from "react-router-dom";
import {
  Title,
  ErrorCard,
  Spinner,
  Paginator,
  EmptyList,
  ExpandableCard,
} from "../../molecules";
import { CreateButtonClassic } from "../../molecules/CreateButton";
import { useItemsRecommenders } from "../../../api-hooks/itemsRecommendersApi";
import { JsonView } from "../../molecules/JsonView";

const ItemsRecommenderRow = ({ recommender }) => {
  return (
    <ExpandableCard label={recommender.name}>
      <Link to={`/recommenders/items-recommenders/detail/${recommender.id}`}>
        <button className="float-right btn btn-primary">View Details</button>
      </Link>
      <JsonView data={recommender} />
    </ExpandableCard>
  );
};
export const ItemsRecommendersSummary = () => {
  const itemsRecommenders = useItemsRecommenders();
  return (
    <React.Fragment>
      <CreateButtonClassic
        className="float-right"
        to="/recommenders/items-recommenders/create"
      >
        Create
      </CreateButtonClassic>
      <Title>Item Recommenders</Title>

      <hr />
      {itemsRecommenders.error && <ErrorCard error={itemsRecommenders.error} />}
      {itemsRecommenders.loading && <Spinner />}
      {itemsRecommenders.items && itemsRecommenders.items.length === 0 && (
        <EmptyList>
          <div className="text-muted m-3">There are no item recommenders.</div>
          <CreateButtonClassic to="/recommenders/items-recommenders/create">
            Create
          </CreateButtonClassic>
        </EmptyList>
      )}

      {itemsRecommenders.items &&
        itemsRecommenders.items.map((pr) => (
          <ItemsRecommenderRow key={pr.id} recommender={pr} />
        ))}

      {itemsRecommenders.pagination && (
        <Paginator {...itemsRecommenders.pagination} />
      )}
    </React.Fragment>
  );
};
