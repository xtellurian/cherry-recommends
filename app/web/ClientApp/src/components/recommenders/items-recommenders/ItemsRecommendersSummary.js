import React from "react";
import { useHistory } from "react-router-dom";
import {
  Title,
  ErrorCard,
  Spinner,
  Paginator,
  EmptyList,
} from "../../molecules";
import { CreateButtonClassic } from "../../molecules/CreateButton";
import { usePromotionsRecommenders } from "../../../api-hooks/promotionsRecommendersApi";
import { ButtonGroup } from "../../molecules/buttons/ButtonGroup";
import { RecommenderRow } from "../RecommenderRow";
import { tabs } from "./ItemsRecommenderPrimaryNav";

const ItemsRecommenderRow = ({ recommender }) => {
  const history = useHistory();

  const handleNavigate = (page) => {
    const queryParams = new URLSearchParams(history.location.search);
    queryParams.set("tab", page);

    history.push({
      ...history.location,
      pathname: `/recommenders/promotions-recommenders/${page}/${recommender.id}`,
      search: queryParams.toString(),
    });
  };

  return (
    <RecommenderRow recommender={recommender}>
      <div>
        <ButtonGroup>
          {tabs.map((tab) => (
            <div
              className="btn btn-outline-primary"
              onClick={() => handleNavigate(tab.id)}
            >
              {tab.label}
            </div>
          ))}
        </ButtonGroup>
      </div>
    </RecommenderRow>
  );
};
export const ItemsRecommendersSummary = () => {
  const itemsRecommenders = usePromotionsRecommenders();
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
