import React from "react";
import { useParams, Link } from "react-router-dom";
import { Link45deg } from "react-bootstrap-icons";
import {
  Spinner,
  ErrorCard,
  EmptyList,
  ExpandableCard,
  Paginator,
} from "../../molecules";
import { JsonView } from "../../molecules/JsonView";
import { DateTimeField } from "../../molecules/DateTimeField";
import {
  useItemsRecommendations,
  useItemsRecommender,
} from "../../../api-hooks/itemsRecommendersApi";

const RecommendationRow = ({ recommendation }) => {
  const dataSubset = {
    recommendationCorrelatorId: recommendation.recommendationCorrelatorId,
    trackedUser: recommendation.trackedUser,
    scoredItems: recommendation.scoredItems,
  };
  let label = `Correlator: ${recommendation.recommendationCorrelatorId}`;
  if (recommendation.trackedUser && recommendation.product) {
    label = `${recommendation.product.name} for ${
      recommendation.trackedUser.name ||
      recommendation.trackedUser.commonId ||
      "user"
    }`;
  }
  return (
    <ExpandableCard label={label}>
      {recommendation.trackedUser && (
        <Link
          target="_blank"
          to={`/tracked-users/detail/${recommendation.trackedUser.id}`}
        >
          <button className="btn btn-primary float-right">
            View Tracked User <Link45deg />
          </button>
        </Link>
      )}
      <DateTimeField label="Created" date={recommendation.created} />
      <JsonView data={recommendation}></JsonView>
    </ExpandableCard>
  );
};
export const RecommendationList = () => {
  const { id } = useParams();
  const recommender = useItemsRecommender({ id });
  const recommendations = useItemsRecommendations({ id });
  return (
    <React.Fragment>
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
