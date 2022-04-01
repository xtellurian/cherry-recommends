import React from "react";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";
import { RecommenderCard } from "../RecommenderCard";
import { DeliveryChannels } from "./DeliveryChannels";

export const Delivery = (recommender) => {
  return (
    <React.Fragment>
      <ItemRecommenderLayout>
        <RecommenderCard title="Channels">
          <DeliveryChannels recommender={recommender} />
        </RecommenderCard>
      </ItemRecommenderLayout>
    </React.Fragment>
  );
};
