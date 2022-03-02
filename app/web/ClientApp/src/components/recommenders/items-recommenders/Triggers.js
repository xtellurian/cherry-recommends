import React from "react";
import { TriggersUtil } from "../utils/triggersUtil";
import { useParams } from "react-router-dom";
import {
  usePromotionsRecommender,
  useTrigger,
} from "../../../api-hooks/promotionsRecommendersApi";
import { setTriggerAsync } from "../../../api/promotionsRecommendersApi";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";
import { Spinner } from "../../molecules";

export const Triggers = () => {
  const { id } = useParams();
  var recommender = usePromotionsRecommender({ id });
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
    <ItemRecommenderLayout>
      {triggerCollection.loading && <Spinner />}
      {!triggerCollection.loading && (
        <TriggersUtil
          error={error}
          recommender={recommender}
          basePath="/recommenders/promotions-recommenders"
          triggerCollection={triggerCollection}
          setTriggerAsync={handleSetTriggerAsync}
        />
      )}
    </ItemRecommenderLayout>
  );
};
