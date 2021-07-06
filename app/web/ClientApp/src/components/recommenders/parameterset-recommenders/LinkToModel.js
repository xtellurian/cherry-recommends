import React from "react";
import { useParams } from "react-router-dom";
import {
  useParameterSetRecommender,
  useLinkedRegisteredModel,
} from "../../../api-hooks/parameterSetRecommendersApi";
import { createLinkRegisteredModel } from "../../../api/parameterSetRecommendersApi";
import { LinkToModelUtility } from "../utils/LinkToModel";
export const LinkToModel = () => {
  const { id } = useParams();
  const recommender = useParameterSetRecommender({ id });
  const linkedModel = useLinkedRegisteredModel({ id });
  return (
    <LinkToModelUtility
      recommender={recommender}
      createLinkRegisteredModel={createLinkRegisteredModel}
      rootPath="/recommenders/parameter-set-recommenders"
      linkedModel={linkedModel}
    />
  );
};
