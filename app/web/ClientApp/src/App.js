import React from "react";

import { configure } from "./api/customisation";
import { Switch, Route } from "react-router-dom";
import { TenantProviderContainer } from "./components/tenants/TenantProviderContainer";
import InTenantApp from "./InTenantApp";
import { Profile } from "./components/auth0/Profile";
import AuthorizeRoute from "./components/auth0/ProtectedRoute";
import "./global-css/cherry.css";
import ManagementApp from "./ManagementApp";
import MultiTenantHome from "./MultiTenantHome";
import { AnonymousSwitcher } from "./components/anonymous/AnonymousSwitcher";
import { ConnectComponent } from "./components/connect/ConnectComponent";
import { Layout } from "./components/Layout";

configure();
const App = ({ multitenant }) => {
  console.debug(
    `App running in ${multitenant ? "multi-tenant" : "single-tenant"} mode`
  );
  if (!multitenant) {
    return (
      <AnonymousSwitcher>
        <Layout multitenant={false}>
          <Route path="/_connect">
            <ConnectComponent />
          </Route>
          <InTenantApp multitenant={false} />
        </Layout>
      </AnonymousSwitcher>
    );
  } else
    return (
      <AnonymousSwitcher>
        <Switch>
          <Route path="/_manage">
            <ManagementApp />
          </Route>
          <Route path="/_connect">
            <ConnectComponent />
          </Route>
          <Route path="/" exact>
            <MultiTenantHome />
          </Route>
          <Route path="/:tenant">
            <TenantProviderContainer>
              <Layout multitenant={true}>
                <InTenantApp multitenant={true} />
              </Layout>
            </TenantProviderContainer>
          </Route>
          <AuthorizeRoute exact path="/_profile" component={Profile} />
        </Switch>
      </AnonymousSwitcher>
    );
};

export default App;
