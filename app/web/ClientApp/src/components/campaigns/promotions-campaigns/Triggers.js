import React from "react";
import { TriggersUtil } from "../utils/triggersUtil";
import { useParams } from "react-router-dom";
import {
  usePromotionsCampaign,
  useTrigger,
} from "../../../api-hooks/promotionsCampaignsApi";
import { setTriggerAsync } from "../../../api/promotionsCampaignsApi";
import { Spinner } from "../../molecules";

export const Triggers = () => {
  const { id } = useParams();
  var recommender = usePromotionsCampaign({ id });
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
    <React.Fragment>
      {triggerCollection.loading && <Spinner />}
      {!triggerCollection.loading && (
        <TriggersUtil
          error={error}
          recommender={recommender}
          basePath="/campaigns/promotions-campaigns"
          triggerCollection={triggerCollection}
          setTriggerAsync={handleSetTriggerAsync}
        />
      )}
    </React.Fragment>
  );
};
