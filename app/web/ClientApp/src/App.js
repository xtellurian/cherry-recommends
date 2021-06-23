import React, { Component } from "react";
import { Route } from "react-router";
import { Layout } from "./components/Layout";
import { Home } from "./components/Home";
import { TrackedUsersComponent } from "./components/tracked-users/TrackedUsersComponent";
import { ExperimentsComponent } from "./components/experiments/ExperimentsComponent";
import AuthorizeRoute from "./components/auth0/ProtectedRoute";
import { SegmentsComponent } from "./components/segments/SegmentsComponent";
import { OffersComponent } from "./components/offers/OffersComponent";
import { ModelRegistrationsComponent } from "./components/models/ModelRegistrationsComponent";
import { ApiDocs } from "./components/docs/ApiDocs";
import { Profile } from "./components/auth0/Profile";
import { SettingsComponent } from "./components/settings/SettingsComponent";
import { DataViewComponent } from "./components/data/DataViewComponent";
import { ReportsComponent } from "./components/reports/ReportsComponent";
import { TouchpointsComponent } from "./components/touchpoints/TouchpointsComponent";
import { ParametersComponent } from "./components/parameters/ParametersComponent";
import { RecommendersComponent } from "./components/recommenders/RecommendersComponent";

// import some demo stuff
import { DemoComponent } from "./components/demo-app/DemoComponent";

import "./custom.css";
export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path="/" component={Home} />
        <AuthorizeRoute component={Profile} path="/profile" />
        <AuthorizeRoute
          path="/tracked-users"
          component={TrackedUsersComponent}
        />
        <AuthorizeRoute path="/experiments" component={ExperimentsComponent} />
        <AuthorizeRoute path="/segments" component={SegmentsComponent} />
        <AuthorizeRoute path="/offers" component={OffersComponent} />
        <AuthorizeRoute path="/parameters" component={ParametersComponent} />
        <AuthorizeRoute path="/recommenders" component={RecommendersComponent} />
        <AuthorizeRoute
          path="/models"
          component={ModelRegistrationsComponent}
        />
        <AuthorizeRoute path="/settings" component={SettingsComponent} />

        <AuthorizeRoute path="/demo" component={DemoComponent} />
        <AuthorizeRoute path="/dataview" component={DataViewComponent} />
        <AuthorizeRoute path="/reports" component={ReportsComponent} />
        <AuthorizeRoute path="/touchpoints" component={TouchpointsComponent} />
        <Route path="/docs/api" component={ApiDocs} />
      </Layout>
    );
  }
}
