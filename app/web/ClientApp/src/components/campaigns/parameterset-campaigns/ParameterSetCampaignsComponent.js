import React from "react";
import { Routes, Route } from "react-router-dom";

import { ParameterSetCampaignsSummary } from "./ParameterSetCampaignsSummary";
import { CreateParameterSetCampaign } from "./CreateParameterSetCampaign";
import { ParameterSetCampaignDetail } from "./ParameterSetCampaignDetail";
import { TestParameterSetCampaign } from "./TestParameterSetCampaign";
import { LinkToModel } from "./LinkToModel";
import { Overview } from "./Overview";
import { Settings } from "./Settings";
import { InvokationLogs } from "./InvokationLogs";
import { MonitorParameterSetCampaign } from "./MonitorParameterSetCampaign";
import { Destinations } from "./Destinations";
import { Triggers } from "./Triggers";
import { LearningMetrics } from "./LearningMetrics";
import { Arguments } from "./Arguments";

export const ParameterSetCampaignsComponent = () => {
  return (
    <Routes>
      <Route index element={<ParameterSetCampaignsSummary />} />
      <Route path="create" element={<CreateParameterSetCampaign />} />
      <Route path="detail/:id" element={<ParameterSetCampaignDetail />} />
      <Route path="overview/:id" element={<Overview />} />
      <Route path="test/:id" element={<TestParameterSetCampaign />} />
      <Route path="link-to-model/:id" element={<LinkToModel />} />
      <Route path="invokation-logs/:id" element={<InvokationLogs />} />
      <Route path="monitor/:id" element={<MonitorParameterSetCampaign />} />
      <Route path="destinations/:id" element={<Destinations />} />
      <Route path="learning-metrics/:id" element={<LearningMetrics />} />
      <Route path="arguments/:id" element={<Arguments />} />
      <Route path="triggers/:id" element={<Triggers />} />
      <Route path="settings/:id" element={<Settings />} />
    </Routes>
  );
};
