import React from "react";
import { useLocation } from "react-router-dom";
import { useParams } from "react-router";
import { PrimaryBackButton } from "../../molecules/BackButton";
import { PageHeading } from "../../molecules";
import { ItemsRecommenderPrimaryNav } from "./ItemsRecommenderPrimaryNav";
import { usePromotionsRecommender } from "../../../api-hooks/promotionsRecommendersApi";
import { RecommenderStatusBox } from "../../molecules/RecommenderStatusBox";

export const ItemRecommenderLayout = ({ children }) => {
  const location = useLocation();
  const { id } = useParams();
  const recommender = usePromotionsRecommender({ id });

  return (
    <>
      <RecommenderStatusBox className="float-right" recommender={recommender} />
      <PrimaryBackButton
        to={{
          ...location,
          pathname: "/recommenders/promotions-recommenders",
        }}
      >
        Back to Recommenders
      </PrimaryBackButton>
      <PageHeading
        title={recommender.name || "..."}
        subtitle="Promotion Recommender"
      />
      <ItemsRecommenderPrimaryNav id={id} />
      {children}
    </>
  );
};
