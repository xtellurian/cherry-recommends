import React from "react";
import { useParams } from "react-router-dom";
import {
  usePromotionsRecommender,
  useLinkedRegisteredModel,
} from "../../../api-hooks/promotionsRecommendersApi";
import { createLinkRegisteredModelAsync } from "../../../api/promotionsRecommendersApi";
import { LinkToModelUtility } from "../utils/LinkToModel";
export const LinkToModel = () => {
  const { id } = useParams();
  const recommender = usePromotionsRecommender({ id });
  const linkedModel = useLinkedRegisteredModel({ id });
  return (
    <LinkToModelUtility
      recommender={recommender}
      createLinkRegisteredModelAsync={createLinkRegisteredModelAsync}
      rootPath="/recommenders/promotions-recommenders"
      linkedModel={linkedModel}
    />
  );
};
