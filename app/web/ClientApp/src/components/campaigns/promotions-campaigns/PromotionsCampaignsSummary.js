import React from "react";
import { Spinner, Paginator, EmptyList } from "../../molecules";
import { CreateButtonClassic } from "../../molecules/CreateButton";
import { usePromotionsCampaigns } from "../../../api-hooks/promotionsCampaignsApi";
import { CampaignRow } from "../CampaignRow";

import Layout, {
  CreateEntityButton,
} from "../../molecules/layout/EntitySummaryLayout";

const PromotionsCampaignRow = ({ recommender }) => {
  return <CampaignRow recommender={recommender} />;
};
export const PromotionsCampaignsSummary = () => {
  const promoCampaigns = usePromotionsCampaigns();
  return (
    <Layout
      header="Promotion Campaigns"
      createButton={
        <CreateEntityButton to="/campaigns/promotions-campaigns/create">
          Create a Campaign
        </CreateEntityButton>
      }
      error={promoCampaigns.error}
    >
      {promoCampaigns.loading && <Spinner />}
      {promoCampaigns.items && promoCampaigns.items.length === 0 && (
        <EmptyList>
          <div className="text-muted m-3">
            There are no promotion campaigns.
          </div>
          <CreateButtonClassic to="/campaigns/promotions-campaigns/create">
            Create
          </CreateButtonClassic>
        </EmptyList>
      )}

      {promoCampaigns.items &&
        promoCampaigns.items.map((pr) => (
          <PromotionsCampaignRow key={pr.id} recommender={pr} />
        ))}

      {promoCampaigns.pagination && (
        <Paginator {...promoCampaigns.pagination} />
      )}
    </Layout>
  );
};
