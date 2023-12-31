import React from "react";
import { useParams } from "react-router-dom";
import { Spinner, ErrorCard, EmptyList, Paginator } from "../../molecules";
import {
  usePromotionsRecommendations,
  usePromotionsCampaign,
} from "../../../api-hooks/promotionsCampaignsApi";
import { RecommendationRow } from "../../recommendations/RecommendationRow";

export const ItemsRecommendationList = ({ size, trigger }) => {
  const { id } = useParams();
  const recommender = usePromotionsCampaign({ id });
  const recommendations = usePromotionsRecommendations({
    id,
    pageSize: 5,
    trigger,
  });

  return (
    <React.Fragment>
      {recommendations.loading && <Spinner />}
      {recommendations.error && <ErrorCard error={recommendations.error} />}
      {recommendations.items && recommendations.items.length === 0 && (
        <EmptyList>This campaign is yet to make a recommendation.</EmptyList>
      )}
      {recommendations.items &&
        recommendations.items.map((r) => (
          <RecommendationRow key={r.id} recommendation={r} />
        ))}
      {recommendations.pagination && size !== "sm" && (
        <Paginator {...recommendations.pagination} />
      )}
    </React.Fragment>
  );
};
