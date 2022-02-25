import React from "react";
import { useLatestRecommendations } from "../../api-hooks/customersApi";
import { Spinner, EmptyList } from "../molecules";
import { RecommendationRow } from "../recommendations/RecommendationRow";

export const LatestRecommendationsSection = ({ trackedUser }) => {
  const latestRecommendations = useLatestRecommendations({
    id: trackedUser.id,
  });

  if (trackedUser.loading || latestRecommendations.loading) {
    return <Spinner />;
  }
  return (
    <React.Fragment>
      {latestRecommendations.items &&
        latestRecommendations.items.length > 0 &&
        latestRecommendations.items.map((r, i) => (
          <RecommendationRow key={i} recommendation={r} />
        ))}
      {latestRecommendations.items &&
        latestRecommendations.items.length === 0 && (
          <EmptyList>
            This Customer is yet to receive a recommendation
          </EmptyList>
        )}
    </React.Fragment>
  );
};
