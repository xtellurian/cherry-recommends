import React from "react";
import { Routes, Route } from "react-router-dom";

import { ParameterSetCampaignsComponent } from "./parameterset-campaigns/ParameterSetCampaignsComponent";
import { PromotionsCampaignsComponent } from "./promotions-campaigns/PromotionsCampaignsComponent";

export const CampaignsComponent = () => {
  return (
    <Routes>
      <Route
        path="parameter-set-campaigns/*"
        element={<ParameterSetCampaignsComponent />}
      />
      <Route
        path="promotions-campaigns/*"
        element={<PromotionsCampaignsComponent />}
      />
    </Routes>
  );
};
