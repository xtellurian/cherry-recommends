import React from "react";
import { Link, useParams } from "react-router-dom";
import { Link45deg } from "react-bootstrap-icons";
import {
  useParameterSetRecommender,
  useParameterSetRecommendations,
} from "../../../api-hooks/parameterSetRecommendersApi";
import {
  BackButton,
  Title,
  Subtitle,
  Spinner,
  ErrorCard,
  EmptyList,
  ExpandableCard,
  Paginator,
} from "../../molecules";
import { JsonView } from "../../molecules/JsonView";
import { DateTimeField } from "../../molecules/DateTimeField";

const RecommendationRow = ({ recommendation }) => {
  const dataToShow = {
    trackedUser: recommendation.trackedUser,
    modelOutput: JSON.parse(recommendation.modelOutput),
    correlatorId: recommendation.recommendationCorrelatorId,
  };
  dataToShow.modelOutput.CorrelatorId =
    recommendation.recommendationCorrelatorId; // patch this value

  let label = `Correlator: ${recommendation.recommendationCorrelatorId}`;
  if (recommendation.trackedUser) {
    label = `Recommendation for ${
      recommendation.trackedUser.name ||
      recommendation.trackedUser.commonId ||
      "user"
    }`;
  }
  return (
    <ExpandableCard label={label}>
      {recommendation.trackedUser && (
        <Link to={`/tracked-users/detail/${recommendation.trackedUser.id}`}>
          <button className="btn btn-primary float-right">
            View Tracked User <Link45deg />
          </button>
        </Link>
      )}
      <DateTimeField label="Created" date={recommendation.created} />
      <JsonView data={dataToShow}></JsonView>
    </ExpandableCard>
  );
};
export const RecommendationList = () => {
  const { id } = useParams();
  const recommender = useParameterSetRecommender({ id });
  const recommendations = useParameterSetRecommendations({ id });
  return (
    <React.Fragment>
      {/* <BackButton
        className="float-right mr-1"
        to={`/recommenders/parameter-set-recommenders/detail/${id}`}
      >
        Recommender
      </BackButton>
      <Title>Latest Recommendations</Title>
      <Subtitle>{recommender.name || "..."}</Subtitle>
      <hr /> */}

      {recommendations.loading && <Spinner />}
      {recommendations.error && <ErrorCard error={recommendations.error} />}
      {recommendations.items && recommendations.items.length === 0 && (
        <EmptyList>This recommender is yet to make a recommendation.</EmptyList>
      )}
      {recommendations.items &&
        recommendations.items.map((r) => (
          <RecommendationRow recommendation={r} key={r.id} />
        ))}
      {recommendations.pagination && (
        <Paginator {...recommendations.pagination} />
      )}
    </React.Fragment>
  );
};
