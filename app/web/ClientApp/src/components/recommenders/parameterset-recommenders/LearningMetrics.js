import React from "react";
import { useParams } from "react-router";
import {
  useLearningMetrics,
  useParameterSetRecommender,
} from "../../../api-hooks/parameterSetRecommendersApi";

import { LearningMetricsUtil } from "../utils/learningMetricsUtil";
import { Spinner } from "../../molecules";
import { setLearningMetricsAsync } from "../../../api/parameterSetRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import { ParameterSetRecommenderLayout } from "./ParameterSetRecommenderLayout";

export const LearningMetrics = () => {
  const { id } = useParams();
  const recommender = useParameterSetRecommender({ id });
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
    <ParameterSetRecommenderLayout>
      {(recommender.loading || learningMetrics.loading) && <Spinner />}
      {!recommender.loading && !learningMetrics.loading && (
        <LearningMetricsUtil
          recommender={recommender}
          learningMetrics={learningMetrics}
          error={error}
          setLearningMetrics={handleSetLearningMetrics}
        />
      )}
    </ParameterSetRecommenderLayout>
  );
};
