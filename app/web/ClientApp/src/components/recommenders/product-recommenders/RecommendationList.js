import React from "react";
import { useParams } from "react-router-dom";
import {
  useProductRecommender,
  useProductRecommendations,
} from "../../../api-hooks/productRecommendersApi";
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
  const dataSubset = {
    recommendationCorrelatorId: recommendation.recommendationCorrelatorId,
    trackedUser: recommendation.trackedUser,
    product: recommendation.product,
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
      <DateTimeField label="Created" date={recommendation.created} />
      <JsonView data={dataSubset}></JsonView>
    </ExpandableCard>
  );
};
export const RecommendationList = () => {
  const { id } = useParams();
  const recommender = useProductRecommender({ id });
  const recommendations = useProductRecommendations({ id });
  return (
    <React.Fragment>
      <BackButton
        className="float-right mr-1"
        to={`/recommenders/product-recommenders/detail/${id}`}
      >
        Recommender
      </BackButton>
      <Title>Latest Recommendations</Title>
      <Subtitle>{recommender.name || "..."}</Subtitle>
      <hr />

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
