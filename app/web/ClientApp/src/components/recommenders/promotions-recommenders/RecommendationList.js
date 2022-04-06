import React from "react";
import { useParams } from "react-router-dom";
import { Spinner, ErrorCard, EmptyList, Paginator } from "../../molecules";
import {
  usePromotionsRecommendations,
  usePromotionsRecommender,
} from "../../../api-hooks/promotionsRecommendersApi";
import { RecommendationRow } from "../../recommendations/RecommendationRow";

const RecommendationRowTemp = ({ recommendation, size }) => {
  return <RecommendationRow recommendation={recommendation} />;
};
export const ItemsRecommendationList = ({ size }) => {
  const { id } = useParams();
  const recommender = usePromotionsRecommender({ id });
  const recommendations = usePromotionsRecommendations({ id, pageSize: 5 });

  return (
    <React.Fragment>
      {recommendations.loading && <Spinner />}
      {recommendations.error && <ErrorCard error={recommendations.error} />}
      {recommendations.items && recommendations.items.length === 0 && (
        <EmptyList>This recommender is yet to make a recommendation.</EmptyList>
      )}
      {recommendations.items &&
        recommendations.items.map((r) => (
          <RecommendationRowTemp size={size} recommendation={r} key={r.id} />
        ))}
      {recommendations.pagination && size !== "sm" && (
        <Paginator {...recommendations.pagination} />
      )}
    </React.Fragment>
  );
};
