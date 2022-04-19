import React from "react";
import { PromotionRecommenderLayout } from "./PromotionRecommenderLayout";
import { RecommenderCard } from "../RecommenderCard";
import { DeliveryChannels } from "./DeliveryChannels";

export const Delivery = (recommender) => {
  return (
    <React.Fragment>
      <PromotionRecommenderLayout>
        <RecommenderCard title="Channels">
          <DeliveryChannels recommender={recommender} />
        </RecommenderCard>
      </PromotionRecommenderLayout>
    </React.Fragment>
  );
};
