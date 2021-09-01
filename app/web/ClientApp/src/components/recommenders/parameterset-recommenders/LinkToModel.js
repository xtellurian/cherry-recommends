import React from "react";
import { useParams } from "react-router-dom";
import {
  useParameterSetRecommender,
  useLinkedRegisteredModel,
} from "../../../api-hooks/parameterSetRecommendersApi";
import { createLinkRegisteredModelAsync } from "../../../api/parameterSetRecommendersApi";
import { LinkToModelUtility } from "../utils/LinkToModel";
export const LinkToModel = () => {
  const { id } = useParams();
  const recommender = useParameterSetRecommender({ id });
  const linkedModel = useLinkedRegisteredModel({ id });
  return (
    <LinkToModelUtility
      recommender={recommender}
      createLinkRegisteredModelAsync={createLinkRegisteredModelAsync}
      rootPath="/recommenders/parameter-set-recommenders"
      linkedModel={linkedModel}
    />
  );
};
