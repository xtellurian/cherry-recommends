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
import { JsonView } from "../../molecules/JsonView";
import { RecommenderStatusBox } from "../../molecules/RecommenderStatusBox";

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
            to={`/recommenders/parameter-set-recommenders/integrate/${id}`}
          >
            Technical Integration
          </ActionLink>
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
      {parameterSetRecommender.loading && (
        <Spinner>Loading Recommender</Spinner>
      )}
      {parameterSetRecommender.error && (
        <ErrorCard error={parameterSetRecommender.error} />
      )}

      <div className="row">
        <div className="col-md order-last">
          {!parameterSetRecommender.loading &&
            !parameterSetRecommender.error && (
              <RecommenderStatusBox recommender={parameterSetRecommender} />
            )}
        </div>
        <div className="col-8">
          {!parameterSetRecommender.loading && (
            <JsonView data={parameterSetRecommender} />
          )}
        </div>
      </div>
    </React.Fragment>
  );
};
