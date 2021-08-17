import React from "react";
import { useLatestRecommendations } from "../../api-hooks/trackedUserApi";
import { Spinner, ExpandableCard } from "../molecules";
import { JsonView } from "../molecules/JsonView";

const RecommendationRow = ({ recommendation }) => {
  return (
    <ExpandableCard
      label={`${recommendation.recommenderType} @ ${recommendation.created}`}
    >
      <div className="row">
        <div className="col">
          Input
          <JsonView data={JSON.parse(recommendation.modelInput)} />
        </div>
        <div className="col">
          <JsonView data={JSON.parse(recommendation.modelOutput)} />
        </div>
      </div>
    </ExpandableCard>
  );
};

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
    </React.Fragment>
  );
};
