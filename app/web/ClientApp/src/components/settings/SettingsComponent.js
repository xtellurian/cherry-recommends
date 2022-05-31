import React from "react";
import { Routes, Route } from "react-router-dom";

import { ApiKeyComponent } from "./apiKeys/ApiKeyComponent";
import { IntegrationsComponent } from "./integrations/IntegrationsComponent";
import { DeploymentInfo } from "./deployment/DeploymentInfo";
import { EnvironmentsComponent } from "./environments/EnvironmentsComponent";
import { Navigation } from "../molecules";

const SettingsHome = () => {
  return (
    <React.Fragment>
      <div>
        <Navigation to="/settings/api-keys">
          <button className="btn btn-outline-primary">API Keys</button>
        </Navigation>
      </div>
      <div>
        <Navigation to="/settings/integrations">
          <button className="btn btn-outline-primary">Integrations</button>
        </Navigation>
      </div>
      <div>
        <Navigation to="/settings/environments">
          <button className="btn btn-outline-primary">Environments</button>
        </Navigation>
      </div>
    </React.Fragment>
  );
};

export const SettingsComponent = () => {
  return (
    <Routes>
      <Route index element={<SettingsHome />} />
      <Route path="api-keys/*" element={<ApiKeyComponent />} />
      <Route path="integrations/*" element={<IntegrationsComponent />} />
      <Route path="info" element={<DeploymentInfo />} />
      <Route path="environments/*" element={<EnvironmentsComponent />} />
    </Routes>
  );
};
