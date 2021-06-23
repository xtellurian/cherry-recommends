import React from "react";
import { useParams } from "react-router-dom";
import { useParameterSetRecommender } from "../../../api-hooks/parameterSetRecommendersApi";
import { Title, Subtitle } from "../../molecules/PageHeadings";
import { ErrorCard } from "../../molecules/ErrorCard";
import { Spinner } from "../../molecules/Spinner";
import { JsonView } from "../../molecules/JsonView";

export const ParameterSetRecommenderDetail = () => {
  const { id } = useParams();
  const parameterSetRecommender = useParameterSetRecommender({ id });
  return (
    <React.Fragment>
      <Title>Parameter Set Recommender</Title>
      <Subtitle>{parameterSetRecommender.name || "..."}</Subtitle>
      <hr />
      {parameterSetRecommender.loading && (
        <Spinner>Loading Recommender</Spinner>
      )}
      {parameterSetRecommender.error && (
        <ErrorCard error={parameterSetRecommender.error} />
      )}
      {!parameterSetRecommender.loading && (
        <JsonView data={parameterSetRecommender} />
      )}
    </React.Fragment>
  );
};
