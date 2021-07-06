import React from "react";
import { useParams } from "react-router-dom";
import { useParameterSetRecommender } from "../../../api-hooks/parameterSetRecommendersApi";
import {
  ActionsButton,
  ActionItemsGroup,
  ActionLink,
} from "../../molecules/ActionsButton";
import {
  Title,
  Subtitle,
  ErrorCard,
  Spinner,
  BackButton,
} from "../../molecules";
import { NoteBox } from "../../molecules/NoteBox";
import { JsonView } from "../../molecules/JsonView";

export const ParameterSetRecommenderDetail = () => {
  const { id } = useParams();
  const parameterSetRecommender = useParameterSetRecommender({ id });
  return (
    <React.Fragment>
      <ActionsButton
        to={`/recommenders/parameter-set-recommenders/test/${id}`}
        label="Test"
      >
        <ActionItemsGroup label="Actions">
          <ActionLink
            to={`/recommenders/parameter-set-recommenders/link-to-model/${id}`}
          >
            Link to Model
          </ActionLink>
        </ActionItemsGroup>
      </ActionsButton>
      <BackButton
        className="float-right mr-1"
        to="/recommenders/parameter-set-recommenders/"
      >
        Parameter Set Recommenders
      </BackButton>
      <Title>Parameter Set Recommender</Title>
      <Subtitle>{parameterSetRecommender.name || "..."}</Subtitle>
      <hr />
      {!parameterSetRecommender.loading &&
        !parameterSetRecommender.modelRegistration && (
          <NoteBox label="Status">
            This recommender's model is still in training.
          </NoteBox>
        )}
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
