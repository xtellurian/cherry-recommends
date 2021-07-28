import React from "react";
import { useParams } from "react-router-dom";
import { useParameterSetRecommender } from "../../../api-hooks/parameterSetRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import { updateErrorHandlingAsync } from "../../../api/parameterSetRecommendersApi";
import { SettingsUtil } from "../utils/settingsUtil";
export const Settings = () => {
  const { id } = useParams();
  const [updated, setUpdated] = React.useState({});
  const recommender = useParameterSetRecommender({ id, trigger: updated });
  const token = useAccessToken();
  const handleUpdate = (errorHandling) => {
    if (errorHandling && errorHandling !== recommender.errorHandling) {
      updateErrorHandlingAsync({
        id,
        token,
        errorHandling,
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
      updateErrorHandling={handleUpdate}
    />
  );
};
