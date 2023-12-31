import React from "react";
import { useParams } from "react-router";
import {
  usePromotionsCampaign,
  useLearningMetrics,
} from "../../../api-hooks/promotionsCampaignsApi";

import { LearningMetricsUtil } from "../utils/learningMetricsUtil";
import { Spinner } from "../../molecules";
import { setLearningMetricsAsync } from "../../../api/promotionsCampaignsApi";
import { useAccessToken } from "../../../api-hooks/token";

export const LearningMetrics = () => {
  const { id } = useParams();
  const recommender = usePromotionsCampaign({ id });
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
    <React.Fragment>
      {(recommender.loading || learningMetrics.loading) && <Spinner />}
      {!recommender.loading && !learningMetrics.loading && (
        <LearningMetricsUtil
          recommender={recommender}
          learningMetrics={learningMetrics}
          error={error}
          setLearningMetrics={handleSetLearningMetrics}
        />
      )}
    </React.Fragment>
  );
};
