import React from "react";
import { useParams } from "react-router-dom";
import { useParameterSetCampaign } from "../../../api-hooks/parameterSetCampaignsApi";
import { useAccessToken } from "../../../api-hooks/token";
import { setSettingsAsync } from "../../../api/parameterSetCampaignsApi";
import { SettingsUtil } from "../utils/settingsUtil";
import { ParameterSetCampaignLayout } from "./ParameterSetCampaignLayout";
export const Settings = () => {
  const { id } = useParams();
  const [updated, setUpdated] = React.useState({});
  const recommender = useParameterSetCampaign({ id, trigger: updated });
  const token = useAccessToken();
  const handleUpdate = (settings) => {
    if (settings && settings !== recommender.settings) {
      setSettingsAsync({
        id,
        token,
        settings,
      })
        .then((v) => {
          setUpdated(v);
        })
        .catch((e) => alert(e.title));
    }
  };
  return (
    <ParameterSetCampaignLayout>
      <SettingsUtil
        recommender={recommender}
        basePath="/campaigns/parameter-set-campaigns"
        updateSettings={handleUpdate}
      />
    </ParameterSetCampaignLayout>
  );
};
