import React from "react";
import { useParams } from "react-router";
import {
  useLearningFeatures,
  useParameterSetRecommender,
} from "../../../api-hooks/parameterSetRecommendersApi";

import { LearningFeaturesUtil } from "../utils/learningFeaturesUtil";
import { Spinner } from "../../molecules";
import { setLearningFeaturesAsync } from "../../../api/parameterSetRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import { ParameterSetRecommenderLayout } from "./ParameterSetRecommenderLayout";

export const LearningFeatures = () => {
  const { id } = useParams();
  const recommender = useParameterSetRecommender({ id });
  const token = useAccessToken();

  const [error, setError] = React.useState();
  const [trigger, setTrigger] = React.useState({});
  const learningFeatures = useLearningFeatures({ id, trigger });
  const handleSetLearningFeatures = (featureIds) => {
    setError(null);
    setLearningFeaturesAsync({ id, token, featureIds })
      .then(setTrigger)
      .catch(setError);
  };

  return (
    <ParameterSetRecommenderLayout>
      {(recommender.loading || learningFeatures.loading) && <Spinner />}
      {!recommender.loading && !learningFeatures.loading && (
        <LearningFeaturesUtil
          recommender={recommender}
          learningFeatures={learningFeatures}
          error={error}
          setLearningFeatures={handleSetLearningFeatures}
        />
      )}
    </ParameterSetRecommenderLayout>
  );
};
