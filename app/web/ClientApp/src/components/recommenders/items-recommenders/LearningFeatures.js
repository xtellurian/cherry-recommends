import React from "react";
import { useParams } from "react-router";
import {
  useItemsRecommender,
  useLearningFeatures,
} from "../../../api-hooks/itemsRecommendersApi";

import { LearningFeaturesUtil } from "../utils/learningFeaturesUtil";
import { Spinner } from "../../molecules";
import { setLearningFeaturesAsync } from "../../../api/itemsRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";

export const LearningFeatures = () => {
  const { id } = useParams();
  const recommender = useItemsRecommender({ id });
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
    <ItemRecommenderLayout>
      {(recommender.loading || learningFeatures.loading) && <Spinner />}
      {!recommender.loading && !learningFeatures.loading && (
        <LearningFeaturesUtil
          recommender={recommender}
          learningFeatures={learningFeatures}
          error={error}
          setLearningFeatures={handleSetLearningFeatures}
        />
      )}
    </ItemRecommenderLayout>
  );
};
