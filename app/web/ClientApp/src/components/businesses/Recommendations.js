import React from "react";
import { useRecommendations } from "../../api-hooks/businessesApi";
import { EmptyList, Spinner } from "../molecules";
import { RecommendationRow } from "../recommendations/RecommendationRow";

export const Recommendations = ({ business } = {}) => {
  const recommendations = useRecommendations({
    id: business.id,
  });

  if (business.loading || recommendations.loading) {
    return <Spinner />;
  }
  return (
    <React.Fragment>
      {recommendations.items &&
        recommendations.items.length > 0 &&
        recommendations.items.map((r, i) => (
          <RecommendationRow key={i} recommendation={r} />
        ))}
      {recommendations.items && recommendations.items.length === 0 && (
        <EmptyList>This Business is yet to receive a recommendation</EmptyList>
      )}
    </React.Fragment>
  );
};
