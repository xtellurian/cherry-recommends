import React from "react";
import {
  Title,
  ErrorCard,
  Spinner,
  Paginator,
  EmptyList,
} from "../../molecules";
import { CreateButtonClassic } from "../../molecules/CreateButton";
import { usePromotionsRecommenders } from "../../../api-hooks/promotionsRecommendersApi";
import { RecommenderRow } from "../RecommenderRow";

const ItemsRecommenderRow = ({ recommender }) => {
  return <RecommenderRow recommender={recommender} />;
};
export const ItemsRecommendersSummary = () => {
  const itemsRecommenders = usePromotionsRecommenders();
  return (
    <React.Fragment>
      <CreateButtonClassic
        className="float-right"
        to="/recommenders/promotions-recommenders/create"
      >
        Create Promotion Campaign
      </CreateButtonClassic>
      <Title>Promotion Campaigns</Title>

      <hr />
      {itemsRecommenders.error && <ErrorCard error={itemsRecommenders.error} />}
      {itemsRecommenders.loading && <Spinner />}
      {itemsRecommenders.items && itemsRecommenders.items.length === 0 && (
        <EmptyList>
          <div className="text-muted m-3">
            There are no promotion campaigns.
          </div>
          <CreateButtonClassic to="/recommenders/promotions-recommenders/create">
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
