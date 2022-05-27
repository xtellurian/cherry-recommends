import React from "react";
import {
  Title,
  ErrorCard,
  Spinner,
  Paginator,
  EmptyList,
} from "../../molecules";
import { CreateButtonClassic } from "../../molecules/CreateButton";
import { usePromotionsCampaigns } from "../../../api-hooks/promotionsCampaignsApi";
import { CampaignRow } from "../CampaignRow";

const PromotionsCampaignRow = ({ recommender }) => {
  return <CampaignRow recommender={recommender} />;
};
export const PromotionsCampaignsSummary = () => {
  const itemsRecommenders = usePromotionsCampaigns();
  return (
    <React.Fragment>
      <CreateButtonClassic
        className="float-right"
        to="/campaigns/promotions-campaigns/create"
      >
        Create Promotion Campaign
      </CreateButtonClassic>
      <Title>Promotion Campaigns</Title>

      <hr />
      {itemsRecommenders.error && <ErrorCard error={itemsRecommenders.error} />}
      {itemsRecommenders.loading && <Spinner />}
      {itemsRecommenders.items && itemsRecommenders.items.length === 0 && (
        <EmptyList>
          <div className="text-muted m-3">
            There are no promotion campaigns.
          </div>
          <CreateButtonClassic to="/campaigns/promotions-campaigns/create">
            Create
          </CreateButtonClassic>
        </EmptyList>
      )}

      {itemsRecommenders.items &&
        itemsRecommenders.items.map((pr) => (
          <PromotionsCampaignRow key={pr.id} recommender={pr} />
        ))}

      {itemsRecommenders.pagination && (
        <Paginator {...itemsRecommenders.pagination} />
      )}
    </React.Fragment>
  );
};
