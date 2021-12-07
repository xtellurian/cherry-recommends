import React from "react";
import { TriggersUtil } from "../utils/triggersUtil";
import { useParams } from "react-router-dom";
import {
  useParameterSetRecommender,
  useTrigger,
} from "../../../api-hooks/parameterSetRecommendersApi";
import { setTriggerAsync } from "../../../api/parameterSetRecommendersApi";
import { Spinner } from "../../molecules";
import { ParameterSetRecommenderLayout } from "./ParameterSetRecommenderLayout";

export const Triggers = () => {
  const { id } = useParams();
  var recommender = useParameterSetRecommender({ id });
  const [reloadTrigger, setReloadTrigger] = React.useState();
  var triggerCollection = useTrigger({ id, trigger: reloadTrigger });
  const [error, setError] = React.useState();

  const handleSetTriggerAsync = async (t) => {
    setError(null);
    try {
      setReloadTrigger(await setTriggerAsync(t));
    } catch (e) {
      setError(e);
    }
  };

  return (
    <ParameterSetRecommenderLayout>
      {triggerCollection.loading && <Spinner />}
      {!triggerCollection.loading && (
        <TriggersUtil
          error={error}
          recommender={recommender}
          basePath="/recommenders/parameter-set-recommenders"
          triggerCollection={triggerCollection}
          setTriggerAsync={handleSetTriggerAsync}
        />
      )}
    </ParameterSetRecommenderLayout>
  );
};
