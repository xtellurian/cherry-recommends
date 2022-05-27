import React from "react";
import { Redirect, Route } from "react-router";
import { useRouteMatch } from "react-router-dom";
import { Home } from "./components/Home";
import { CustomersComponent } from "./components/customers/CustomersComponent";
import AuthorizeRoute from "./components/auth0/ProtectedRoute";
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
import "./global-css/cherry.css";
import { ChannelsComponent } from "./components/channels/ChannelsComponent";
import { GettingStartedChecklistComponent } from "./components/onboarding/GettingStartedChecklist";

// TODO: if single tenant, path format must be `/<menu>/<submenu>`
// TODO: if multi tenant, path format must be `/<tenant-name>/<menu>/<submenu>`

const InTenantApp = ({ multitenant }) => {
  const { params } = useRouteMatch();
  let routePrefix = "";
  if (multitenant) {
    routePrefix = `/${params.tenant}`;
  }

  return (
    <React.Fragment>
      <Route exact path={`${routePrefix}/`} component={Home} />
      <AuthorizeRoute
        path={`${routePrefix}/admin`}
        component={AdminComponent}
      />

      {/* TODO: move out of single tenant app?  */}
      <AuthorizeRoute component={Profile} path={`${routePrefix}/profile`} />

      {/* TODO: move to CustomerComponent */}
      <Route path={`${routePrefix}/tracked-users`}>
        <Redirect to={`${routePrefix}/customers`} />
      </Route>

      <AuthorizeRoute
        path={`${routePrefix}/customers`}
        component={CustomersComponent}
      />

      <AuthorizeRoute
        path={`${routePrefix}/metrics`}
        component={MetricsComponent}
      />
      <AuthorizeRoute
        path={`${routePrefix}/parameters`}
        component={ParametersComponent}
      />
      <AuthorizeRoute
        path={`${routePrefix}/campaigns`}
        component={CampaignsComponent}
      />
      <AuthorizeRoute
        path={`${routePrefix}/promotions`}
        component={PromotionsComponent}
      />
      <AuthorizeRoute
        path={`${routePrefix}/events`}
        component={EventsComponent}
      />
      <AuthorizeRoute
        path={`${routePrefix}/settings`}
        component={SettingsComponent}
      />
      <AuthorizeRoute
        path={`${routePrefix}/tenant-settings`}
        component={TenantSettingsComponent}
      />
      <AuthorizeRoute
        path={`${routePrefix}/reports`}
        component={ReportsComponent}
      />
      {/* Note: move to IntegrationsComponent */}
      <AuthorizeRoute
        path={`${routePrefix}/integrations/channels`}
        component={ChannelsComponent}
      />
      <AuthorizeRoute
        path={`${routePrefix}/getting-started`}
        component={GettingStartedChecklistComponent}
      />
      <Route path={`${routePrefix}/docs/api`} component={ApiDocs} />

      <Identifier />
    </React.Fragment>
  );
};

export default InTenantApp;
