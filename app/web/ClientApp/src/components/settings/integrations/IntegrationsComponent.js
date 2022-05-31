import React from "react";
import { Route, Routes } from "react-router-dom";

import { ListConnections, ListDataSources } from "./ListIntegrations";
import { CreateIntegration } from "./CreateIntegration";
import { IntegratedSystemDetail } from "./IntegratedSystemDetail";
import { CreateWebhookReceiver } from "./CreateWebhookReceiver";
import { HubspotConnector } from "./hubspot/HubspotConnector";
import { HubspotIntegrationComponent } from "./hubspot/HubspotIntegrationComponent";
import { CustomIntegrationComponent } from "./custom/CustomIntegrationComponent";

export const IntegrationsComponent = () => {
  return (
    <Routes>
      <Route index element={<ListConnections />} />
      <Route path="data-sources" element={<ListDataSources />} />
      <Route path="create" element={<CreateIntegration />} />
      <Route path="detail/:id" element={<IntegratedSystemDetail />} />
      <Route
        path="create-webhook-receiver/:id"
        element={<CreateWebhookReceiver />}
      />
      <Route path="hubspotconnector" element={<HubspotConnector />} />
      <Route
        path="hubspot-detail/:id"
        element={<HubspotIntegrationComponent />}
      />
      <Route path="custom/:id" element={<CustomIntegrationComponent />} />
    </Routes>
  );
};
