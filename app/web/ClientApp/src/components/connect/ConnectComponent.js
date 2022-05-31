import React from "react";
import { Route, Routes } from "react-router-dom";

import { ShopifyConnector } from "../settings/integrations/shopify/ShopifyConnector";
import { ShopifyInstall } from "../settings/integrations/shopify/ShopifyInstall";

export const ConnectComponent = () => {
  return (
    <Routes>
      <Route path="shopify/callback" element={<ShopifyConnector />} />
      <Route path="shopify/install" element={<ShopifyInstall />} />
    </Routes>
  );
};
