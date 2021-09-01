import React from "react";
import { useParams } from "react-router-dom";
import {
  useItemsRecommender,
  useLinkedRegisteredModel,
} from "../../../api-hooks/itemsRecommendersApi";
import { createLinkRegisteredModelAsync } from "../../../api/itemsRecommendersApi";
import { LinkToModelUtility } from "../utils/LinkToModel";
export const LinkToModel = () => {
  const { id } = useParams();
  const recommender = useItemsRecommender({ id });
  const linkedModel = useLinkedRegisteredModel({ id });
  return (
    <LinkToModelUtility
      recommender={recommender}
      createLinkRegisteredModelAsync={createLinkRegisteredModelAsync}
      rootPath="/recommenders/items-recommenders"
      linkedModel={linkedModel}
    />
  );
};
