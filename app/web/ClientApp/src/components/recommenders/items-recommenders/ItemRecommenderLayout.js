import React from "react";
import { useParams } from "react-router";
import { PrimaryBackButton } from "../../molecules/BackButton";
import { Title, Subtitle } from "../../molecules";
import { ItemsRecommenderPrimaryNav } from "./ItemsRecommenderPrimaryNav";
import { usePromotionsRecommender } from "../../../api-hooks/promotionsRecommendersApi";
import { RecommenderStatusBox } from "../../molecules/RecommenderStatusBox";

export const ItemRecommenderLayout = ({ children }) => {
  const { id } = useParams();
  const recommender = usePromotionsRecommender({ id });
  return (
    <>
      <RecommenderStatusBox className="float-right" recommender={recommender} />
      <PrimaryBackButton to={"/recommenders/promotions-recommenders"}>
        Back to Recommenders
      </PrimaryBackButton>
      <Title>{recommender.name || "..."}</Title>
      <Subtitle>Promotion Recommender</Subtitle>
      <ItemsRecommenderPrimaryNav id={id} />
      {children}
    </>
  );
};
