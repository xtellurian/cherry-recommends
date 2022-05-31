import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";

import { Home } from "./components/Home";
import { CustomersComponent } from "./components/customers/CustomersComponent";
import { ApiDocs } from "./components/docs/ApiDocs";
import { Profile } from "./components/auth0/Profile";
import { SettingsComponent } from "./components/settings/SettingsComponent";
import { ReportsComponent } from "./components/reports/ReportsComponent";
import MetricsComponent from "./components/metrics/MetricsComponent";
import { ParametersComponent } from "./components/parameters/ParametersComponent";
import { CampaignsComponent } from "./components/campaigns/CampaignsComponent";
import PromotionsComponent from "./components/promotions/PromotionsComponent";
import { EventsComponent } from "./components/events/EventsComponent";
import { AdminComponent } from "./components/admin/AdminComponent";
import { TenantSettingsComponent } from "./components/tenant-settings/TenantSettingsComponent";
import Identifier from "./analytics/Identifier";
import { ChannelsComponent } from "./components/channels/ChannelsComponent";
import { GettingStartedChecklistComponent } from "./components/onboarding/GettingStartedChecklist";
import { Layout as PageLayout } from "./components/Layout";
import ProtectedRoute from "./components/auth0/ProtectedRoute";
import { ErrorBoundary } from "./components/molecules/ErrorBoundary";
import { TenantProviderContainer } from "./components/tenants/TenantProviderContainer";

import "./global-css/cherry.css";

// TODO: if single tenant, path format must be `/<menu>/<submenu>`
// TODO: if multi tenant, path format must be `/<tenant-name>/<menu>/<submenu>`
export const InTenantApp = ({ multitenant, specialRoutes }) => {
  return (
    <Routes>
      <Route path={multitenant ? "/:tenant" : "/"}>
        <Route element={<TenantProviderContainer />}>
          <Route element={<PageLayout multitenant={multitenant} />}>
            <Route element={<Identifier />}>
              <Route element={<ErrorBoundary />}>
                <Route index element={<ProtectedRoute component={Home} />} />
                <Route
                  path="admin/*"
                  element={<ProtectedRoute component={AdminComponent} />}
                />
                <Route
                  path="customers/*"
                  element={<ProtectedRoute component={CustomersComponent} />}
                />
                <Route
                  path="metrics/*"
                  element={<ProtectedRoute component={MetricsComponent} />}
                />
                <Route
                  path="parameters/*"
                  element={<ProtectedRoute component={ParametersComponent} />}
                />
                <Route
                  path="campaigns/*"
                  element={<ProtectedRoute component={CampaignsComponent} />}
                />
                <Route
                  path="promotions/*"
                  element={<ProtectedRoute component={PromotionsComponent} />}
                />
                <Route
                  path="events/*"
                  element={<ProtectedRoute component={EventsComponent} />}
                />
                <Route
                  path="settings/*"
                  element={<ProtectedRoute component={SettingsComponent} />}
                />
                <Route
                  path="tenant-settings/*"
                  element={
                    <ProtectedRoute component={TenantSettingsComponent} />
                  }
                />
                <Route
                  path="reports/*"
                  element={<ProtectedRoute component={ReportsComponent} />}
                />

                {/* TODO: Create IntegrationComponent and replace ChannelsComponent with it */}
                <Route
                  path="integrations/*"
                  element={<ProtectedRoute component={ChannelsComponent} />}
                />

                <Route
                  path="getting-started"
                  element={
                    <ProtectedRoute
                      component={GettingStartedChecklistComponent}
                    />
                  }
                />
                <Route
                  path="docs/api"
                  element={<ProtectedRoute component={ApiDocs} />}
                />

                {/* TODO: Move out of single tenant app? */}
                <Route
                  path={multitenant ? "_profile" : "profile"}
                  element={<ProtectedRoute component={Profile} />}
                />

                <Route
                  path="tracked-users"
                  element={<Navigate to="/customers" />}
                />
              </Route>
            </Route>
          </Route>
        </Route>
      </Route>

      {/* special routes are not influenced by :tenant parameter */}
      {/* special routes are outside the PageLayout wrapper */}
      {specialRoutes}
    </Routes>
  );
};

export default InTenantApp;
