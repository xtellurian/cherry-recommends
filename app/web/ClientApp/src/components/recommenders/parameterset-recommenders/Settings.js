import React from "react";
import { useParams } from "react-router-dom";
import { useParameterSetRecommender } from "../../../api-hooks/parameterSetRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import { setSettingsAsync } from "../../../api/parameterSetRecommendersApi";
import { SettingsUtil } from "../utils/settingsUtil";
export const Settings = () => {
  const { id } = useParams();
  const [updated, setUpdated] = React.useState({});
  const recommender = useParameterSetRecommender({ id, trigger: updated });
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
    <SettingsUtil
      recommender={recommender}
      basePath="/recommenders/parameter-set-recommenders"
      updateSettings={handleUpdate}
    />
  );
};
