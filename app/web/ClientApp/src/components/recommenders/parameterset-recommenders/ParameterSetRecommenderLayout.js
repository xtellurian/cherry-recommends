import React from "react";
import { useParams } from "react-router";
import { PrimaryBackButton } from "../../molecules/BackButton";
import { Title, Subtitle } from "../../molecules";
import { ParameterSetRecommenderPrimaryNav } from "./ParameterSetRecommenderPrimaryNav";
import { useParameterSetRecommender } from "../../../api-hooks/parameterSetRecommendersApi";
import { RecommenderStatusBox } from "../../molecules/RecommenderStatusBox";

export const ParameterSetRecommenderLayout = ({ children }) => {
  const { id } = useParams();
  const recommender = useParameterSetRecommender({ id });
  return (
    <>
      <RecommenderStatusBox className="float-right" recommender={recommender} />
      <PrimaryBackButton to={"/recommenders/parameter-set-recommenders"}>
        Back to Recommenders
      </PrimaryBackButton>
      <Title>{recommender.name || "..."}</Title>
      <Subtitle>Parameter Set Recommender</Subtitle>
      <ParameterSetRecommenderPrimaryNav id={id} />
      {children}
    </>
  );
};
