import React from "react";
import { Redirect, Route } from "react-router";
import { Layout } from "./components/Layout";
import { Home } from "./components/Home";
import { CustomersComponent } from "./components/customers/CustomersComponent";
import AuthorizeRoute from "./components/auth0/ProtectedRoute";
import { SegmentsComponent } from "./components/segments/SegmentsComponent";
import { ModelRegistrationsComponent } from "./components/models/ModelRegistrationsComponent";
import { ApiDocs } from "./components/docs/ApiDocs";
import { Profile } from "./components/auth0/Profile";
import { SettingsComponent } from "./components/settings/SettingsComponent";
import { DataViewComponent } from "./components/data/DataViewComponent";
import { ReportsComponent } from "./components/reports/ReportsComponent";
import MetricsComponent from "./components/metrics/MetricsComponent";
import { ParametersComponent } from "./components/parameters/ParametersComponent";
import { RecommendersComponent } from "./components/recommenders/RecommendersComponent";
import { RecommendableItemsComponent } from "./components/items/RecommendableItemsComponent";
import { EventsComponent } from "./components/events/EventsComponent";
import { AdminComponent } from "./components/admin/AdminComponent";
import { TenantSettingsComponent } from "./components/tenant-settings/TenantSettingsComponent";
import { configure } from "./api/customisation";
import Analytics from "./analytics/Analytics";
import Identifier from "./analytics/Identifier";
import "./global-css/cherry.css";
import { BusinessesComponent } from "./components/businesses/BusinessesComponent";

configure();
const App = () => {
  return (
    <Layout>
      <Analytics>
        <Route exact path="/" component={Home} />
        <AuthorizeRoute path="/admin" component={AdminComponent} />
        <AuthorizeRoute component={Profile} path="/profile" />
        <Route path="/tracked-users">
          <Redirect to="/customers" />
        </Route>
        <AuthorizeRoute path="/customers" component={CustomersComponent} />
        <AuthorizeRoute path="/metrics" component={MetricsComponent} />
        <AuthorizeRoute path="/segments" component={SegmentsComponent} />
        <AuthorizeRoute path="/parameters" component={ParametersComponent} />
        <AuthorizeRoute
          path="/recommenders"
          component={RecommendersComponent}
        />
        <AuthorizeRoute
          path="/promotions"
          component={RecommendableItemsComponent}
        />
        <AuthorizeRoute
          path="/models"
          component={ModelRegistrationsComponent}
        />
        <AuthorizeRoute path="/events" component={EventsComponent} />
        <AuthorizeRoute path="/settings" component={SettingsComponent} />

        <AuthorizeRoute
          path="/tenant-settings"
          component={TenantSettingsComponent}
        />

        <AuthorizeRoute path="/dataview" component={DataViewComponent} />
        <AuthorizeRoute path="/reports" component={ReportsComponent} />
        <AuthorizeRoute path="/businesses" component={BusinessesComponent} />
        <Route path="/docs/api" component={ApiDocs} />

        <Identifier />
      </Analytics>
    </Layout>
  );
};

export default App;
