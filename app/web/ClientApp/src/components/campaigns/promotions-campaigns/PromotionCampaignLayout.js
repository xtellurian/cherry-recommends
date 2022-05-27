import React from "react";
import { useLocation } from "react-router-dom";
import { useParams } from "react-router";

import { MoveUpHierarchyPrimaryButton, PageHeading } from "../../molecules";
import { PromotionsCampaignPrimaryNav } from "./PromotionsCampaignPrimaryNav";
import { usePromotionsCampaign } from "../../../api-hooks/promotionsCampaignsApi";
import { CampaignStatusBox } from "../../molecules/CampaignStatusBox";
import { setSettingsAsync } from "../../../api/promotionsCampaignsApi";
import { useAccessToken } from "../../../api-hooks/token";
import EntityDetailPageLayout from "../../molecules/layout/EntityDetailPageLayout";

export const PromotionCampaignLayout = ({ children }) => {
  const location = useLocation();
  const token = useAccessToken();
  const { id } = useParams();
  const [trigger, setTrigger] = React.useState({});
  const recommender = usePromotionsCampaign({ id, trigger });

  const setEnabled = (value) => {
    console.log(value);
    setSettingsAsync({
      id,
      settings: {
        enabled: value,
      },
      token,
    })
      .then(setTrigger)
      .catch(console.error);
  };

  const searchParams = new URLSearchParams(location.search);
  searchParams.delete("tab");

  return (
    <EntityDetailPageLayout
      backButton={
        <MoveUpHierarchyPrimaryButton
          to={{
            pathname: "/campaigns/promotions-campaigns",
            search: searchParams.toString(),
          }}
        >
          Back to Campaigns
        </MoveUpHierarchyPrimaryButton>
      }
      header={
        <PageHeading
          title={recommender.name || "..."}
          subtitle="Promotion Campaign"
        />
      }
      options={
        <CampaignStatusBox recommender={recommender} setEnabled={setEnabled} />
      }
    >
      <PromotionsCampaignPrimaryNav id={id} />
      {children}
    </EntityDetailPageLayout>
  );
};
