import React from "react";
import { Route, Routes } from "react-router-dom";

import { ShopifyConnector } from "../settings/integrations/shopify/ShopifyConnector";
import { ShopifyInstall } from "../settings/integrations/shopify/ShopifyInstall";
import { HubspotCallback } from "../settings/integrations/hubspot/HubspotCallback";

export const ConnectComponent = () => {
  return (
    <Routes>
      <Route path="shopify/callback" element={<ShopifyConnector />} />
      <Route path="shopify/install" element={<ShopifyInstall />} />
      <Route path="hubspot/callback" element={<HubspotCallback />} />
    </Routes>
  );
};
