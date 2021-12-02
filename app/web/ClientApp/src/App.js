import React, { Component } from "react";
import { Route } from "react-router";
import { Layout } from "./components/Layout";
import { Home } from "./components/Home";
import { TrackedUsersComponent } from "./components/tracked-users/TrackedUsersComponent";
import AuthorizeRoute from "./components/auth0/ProtectedRoute";
import { SegmentsComponent } from "./components/segments/SegmentsComponent";
import { ModelRegistrationsComponent } from "./components/models/ModelRegistrationsComponent";
import { ApiDocs } from "./components/docs/ApiDocs";
import { Profile } from "./components/auth0/Profile";
import { SettingsComponent } from "./components/settings/SettingsComponent";
import { DataViewComponent } from "./components/data/DataViewComponent";
import { ReportsComponent } from "./components/reports/ReportsComponent";
import { TouchpointsComponent } from "./components/touchpoints/TouchpointsComponent";
import { FeaturesComponent } from "./components/features/FeaturesComponent";
import { ParametersComponent } from "./components/parameters/ParametersComponent";
import { RecommendersComponent } from "./components/recommenders/RecommendersComponent";
import { RecommendableItemsComponent } from "./components/items/RecommendableItemsComponent";
import { EventsComponent } from "./components/events/EventsComponent";
import { AdminComponent } from "./components/admin/AdminComponent";
import { TenantSettingsComponent } from "./components/tenant-settings/TenantSettingsComponent";
import { configure } from "./api/customisation";
import "./global-css/cherry.css";

configure();
export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path="/" component={Home} />
        <AuthorizeRoute path="/admin" component={AdminComponent} />
        <AuthorizeRoute component={Profile} path="/profile" />
        <AuthorizeRoute
          path="/tracked-users"
          component={TrackedUsersComponent}
        />
        <AuthorizeRoute path="/features" component={FeaturesComponent} />
        <AuthorizeRoute path="/segments" component={SegmentsComponent} />
        <AuthorizeRoute path="/parameters" component={ParametersComponent} />
        <AuthorizeRoute
          path="/recommenders"
          component={RecommendersComponent}
        />
        <AuthorizeRoute
          path="/recommendable-items"
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
        <AuthorizeRoute path="/touchpoints" component={TouchpointsComponent} />
        <Route path="/docs/api" component={ApiDocs} />
      </Layout>
    );
  }
}
