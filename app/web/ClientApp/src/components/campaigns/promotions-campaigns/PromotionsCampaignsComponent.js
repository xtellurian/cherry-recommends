import React from "react";
import { Routes, Route } from "react-router-dom";

import { PromotionsCampaignsSummary } from "./PromotionsCampaignsSummary";
import { CreateCampaign } from "./CreatePromotionsCampaign";
import { CampaignDetail } from "./PromotionsCampaignDetail";
import { TestCampaign } from "./TestPromotionsCampaign";
import { LinkToModel } from "./LinkToModel";
import { TargetVariableValues } from "./TargetVariableValues";
import { InvokationLogs } from "./InvokationLogs";
import { SettingsPage } from "./SettingsPage";
import { MonitorCampaign } from "./MonitorPromotionsCampaign";
import { ManageItems } from "./manage/ManagePromotions";
import { ManageComponent } from "./manage/ManageComponent";
import { ManageAudience } from "./manage/ManageAudience";
// import { Destinations } from "./Destinations";
import { Triggers } from "./Triggers";
import { LearningMetrics } from "./LearningMetrics";
import { Arguments } from "./Arguments";
import { Overview } from "./Overview";
import { Delivery } from "./Delivery";
import Performance from "./PerformanceTable";

export const PromotionsCampaignsComponent = () => {
  return (
    <Routes>
      <Route index element={<PromotionsCampaignsSummary />} />
      <Route path="create" element={<CreateCampaign />} />
      <Route path="overview/:id" element={<Overview />} />
      <Route path="detail/:id" element={<CampaignDetail />} />
      <Route path="link-to-model/:id" element={<LinkToModel />} />
      <Route path="test/:id" element={<TestCampaign />} />
      <Route path="target-variable/:id" element={<TargetVariableValues />} />
      <Route path="invokation-logs/:id" element={<InvokationLogs />} />
      <Route path="monitor/:id" element={<MonitorCampaign />} />
      <Route path="manage-promotions/:id" element={<ManageItems />} />
      <Route path="manage/*" element={<ManageComponent />} />
      <Route path="triggers/:id" element={<Triggers />} />
      <Route path="settings/:id" element={<SettingsPage />} />
      <Route path="learning-metrics/:id" element={<LearningMetrics />} />
      <Route path="arguments/:id" element={<Arguments />} />
      <Route path="reports/:id" element={<Performance />} />
      <Route path="delivery/:id" element={<Delivery />} />
      <Route path="audience/:id" element={<ManageAudience />} />
    </Routes>
  );
};
