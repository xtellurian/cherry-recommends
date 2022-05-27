import React from "react";
import { useParams } from "react-router-dom";
import {
  usePromotionsCampaign,
  useLinkedRegisteredModel,
} from "../../../api-hooks/promotionsCampaignsApi";
import { createLinkRegisteredModelAsync } from "../../../api/promotionsCampaignsApi";
import { LinkToModelUtility } from "../utils/LinkToModel";
export const LinkToModel = () => {
  const { id } = useParams();
  const recommender = usePromotionsCampaign({ id });
  const linkedModel = useLinkedRegisteredModel({ id });
  return (
    <LinkToModelUtility
      recommender={recommender}
      createLinkRegisteredModelAsync={createLinkRegisteredModelAsync}
      rootPath="/campaigns/promotions-campaigns"
      linkedModel={linkedModel}
    />
  );
};
