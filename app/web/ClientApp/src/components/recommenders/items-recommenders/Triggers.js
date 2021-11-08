import React from "react";
import { TriggersUtil } from "../utils/triggersUtil";
import { useParams } from "react-router-dom";
import {
  useItemsRecommender,
  useTrigger,
} from "../../../api-hooks/itemsRecommendersApi";
import { setTriggerAsync } from "../../../api/itemsRecommendersApi";
import { Spinner } from "../../molecules";

export const Triggers = () => {
  const { id } = useParams();
  var recommender = useItemsRecommender({ id });
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
  if (triggerCollection.loading) {
    return <Spinner>Loading Triggers</Spinner>;
  }
  return (
    <TriggersUtil
      error={error}
      recommender={recommender}
      basePath="/recommenders/items-recommenders"
      triggerCollection={triggerCollection}
      setTriggerAsync={handleSetTriggerAsync}
    />
  );
};
