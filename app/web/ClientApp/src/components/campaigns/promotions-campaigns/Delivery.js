import React from "react";
import { PromotionCampaignLayout } from "./PromotionCampaignLayout";
import { CampaignCard } from "../CampaignCard";
import { DeliveryChannels } from "./DeliveryChannels";

export const Delivery = (recommender) => {
  return (
    <React.Fragment>
      <PromotionCampaignLayout>
        <CampaignCard title="Channels">
          <DeliveryChannels recommender={recommender} />
        </CampaignCard>
      </PromotionCampaignLayout>
    </React.Fragment>
  );
};
