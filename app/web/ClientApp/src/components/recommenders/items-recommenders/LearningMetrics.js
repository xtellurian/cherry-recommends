import React from "react";
import { useParams } from "react-router";
import {
  useItemsRecommender,
  useLearningMetrics,
} from "../../../api-hooks/itemsRecommendersApi";

import { LearningMetricsUtil } from "../utils/learningMetricsUtil";
import { Spinner } from "../../molecules";
import { setLearningMetricsAsync } from "../../../api/itemsRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";

export const LearningMetrics = () => {
  const { id } = useParams();
  const recommender = useItemsRecommender({ id });
  const token = useAccessToken();

  const [error, setError] = React.useState();
  const [trigger, setTrigger] = React.useState({});
  const learningMetrics = useLearningMetrics({ id, trigger });
  const handleSetLearningMetrics = (metricIds) => {
    setError(null);
    setLearningMetricsAsync({ id, token, metricIds })
      .then(setTrigger)
      .catch(setError);
  };

  return (
    <ItemRecommenderLayout>
      {(recommender.loading || learningMetrics.loading) && <Spinner />}
      {!recommender.loading && !learningMetrics.loading && (
        <LearningMetricsUtil
          recommender={recommender}
          learningMetrics={learningMetrics}
          error={error}
          setLearningMetrics={handleSetLearningMetrics}
        />
      )}
    </ItemRecommenderLayout>
  );
};
