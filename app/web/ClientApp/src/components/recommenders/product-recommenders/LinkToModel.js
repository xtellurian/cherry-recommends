import React from "react";
import { useParams } from "react-router-dom";
import {
  useProductRecommender,
  useLinkedRegisteredModel,
} from "../../../api-hooks/productRecommendersApi";
import { createLinkRegisteredModelAsync } from "../../../api/productRecommendersApi";
import { LinkToModelUtility } from "../utils/LinkToModel";
export const LinkToModel = () => {
  const { id } = useParams();
  const recommender = useProductRecommender({ id });
  const linkedModel = useLinkedRegisteredModel({ id });
  return (
    <LinkToModelUtility
      recommender={recommender}
      createLinkRegisteredModelAsync={createLinkRegisteredModelAsync}
      rootPath="/recommenders/product-recommenders"
      linkedModel={linkedModel}
    />
  );
};
