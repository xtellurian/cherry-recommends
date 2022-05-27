import React from "react";
import { TriggersUtil } from "../utils/triggersUtil";
import { useParams } from "react-router-dom";
import {
  useParameterSetCampaign,
  useTrigger,
} from "../../../api-hooks/parameterSetCampaignsApi";
import { setTriggerAsync } from "../../../api/parameterSetCampaignsApi";
import { Spinner } from "../../molecules";
import { ParameterSetCampaignLayout } from "./ParameterSetCampaignLayout";

export const Triggers = () => {
  const { id } = useParams();
  var recommender = useParameterSetCampaign({ id });
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
    <ParameterSetCampaignLayout>
      {triggerCollection.loading && <Spinner />}
      {!triggerCollection.loading && (
        <TriggersUtil
          error={error}
          recommender={recommender}
          basePath="/campaigns/parameter-set-campaigns"
          triggerCollection={triggerCollection}
          setTriggerAsync={handleSetTriggerAsync}
        />
      )}
    </ParameterSetCampaignLayout>
  );
};
