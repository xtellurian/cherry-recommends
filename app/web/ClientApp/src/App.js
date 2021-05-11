import React, { Component } from "react";
import { Route } from "react-router";
import { Layout } from "./components/Layout";
import { Home } from "./components/Home";
import { FetchData } from "./components/FetchData";
import { TrackedUsers } from "./components/tracked-users/TrackedUsers";
import { ExperimentsComponent } from "./components/experiments/ExperimentsComponent";
import AuthorizeRoute from "./components/auth0/ProtectedRoute";
import ApiAuthorizationRoutes from "./components/api-authorization/ApiAuthorizationRoutes";
import { ApplicationPaths } from "./components/api-authorization/ApiAuthorizationConstants";
import { SegmentsComponent } from "./components/segments/SegmentsComponent";
import { OffersComponent } from "./components/offers/OffersComponent";
import { Profile } from "./components/auth0/Profile";
import { ApiKeyComponent } from "./components/apiKeys/ApiKeyComponent";

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
        <AuthorizeRoute path="/fetch-data" component={FetchData} />
        <AuthorizeRoute path="/tracked-users" component={TrackedUsers} />
        <AuthorizeRoute path="/experiments" component={ExperimentsComponent} />
        <AuthorizeRoute path="/segments" component={SegmentsComponent} />
        <AuthorizeRoute path="/offers" component={OffersComponent} />
        <AuthorizeRoute path="/api-keys" component={ApiKeyComponent} />

        <AuthorizeRoute path="/demo" component={DemoComponent} />
        <Route
          path={ApplicationPaths.ApiAuthorizationPrefix}
          component={ApiAuthorizationRoutes}
        />
      </Layout>
    );
  }
}
