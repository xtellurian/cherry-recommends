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
import EntityRow from "../../molecules/layout/EntityFlexRow";
import { ButtonGroup } from "../../molecules/buttons/ButtonGroup";

const ItemsRecommenderRow = ({ recommender }) => {
  return (
    <EntityRow>
      <div className="ml-2 align-middle">{recommender.name}</div>
      <div>
        <ButtonGroup>
          <div className="btn btn-outline-primary">
            <Link
              to={`/recommenders/promotions-recommenders/detail/${recommender.id}`}
            >
              <div>Details</div>
            </Link>
          </div>
          <div className="btn btn-outline-primary">
            <Link
              to={`/recommenders/promotions-recommenders/monitor/${recommender.id}`}
            >
              <div>Monitor</div>
            </Link>
          </div>
          <div className="btn btn-outline-primary">
            <Link
              to={`/recommenders/promotions-recommenders/performance/${recommender.id}`}
            >
              <div>Performance</div>
            </Link>
          </div>
          <div className="btn btn-outline-primary">
            <Link
              to={`/recommenders/promotions-recommenders/test/${recommender.id}`}
            >
              <div>Test</div>
            </Link>
          </div>
        </ButtonGroup>
      </div>
    </EntityRow>
  );
};
export const ItemsRecommendersSummary = () => {
  const itemsRecommenders = useItemsRecommenders();
  return (
    <React.Fragment>
      <CreateButtonClassic
        className="float-right"
        to="/recommenders/promotions-recommenders/create"
      >
        Create Promotion Recommender
      </CreateButtonClassic>
      <Title>Promotion Recommenders</Title>

      <hr />
      {itemsRecommenders.error && <ErrorCard error={itemsRecommenders.error} />}
      {itemsRecommenders.loading && <Spinner />}
      {itemsRecommenders.items && itemsRecommenders.items.length === 0 && (
        <EmptyList>
          <div className="text-muted m-3">
            There are no promotion recommenders.
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
