import React from "react";

import { configure } from "./api/customisation";
import { Route } from "react-router-dom";
import ManagementApp from "./ManagementApp";
import MultiTenantHome from "./MultiTenantHome";
import { AnonymousSwitcher } from "./components/anonymous/AnonymousSwitcher";
import { ConnectComponent } from "./components/connect/ConnectComponent";
import InTenantApp from "./InTenantApp";

import "./global-css/cherry.css";

configure();
const App = ({ multitenant }) => {
  console.debug(
    `App running in ${multitenant ? "multi-tenant" : "single-tenant"} mode`
  );

  return (
    <AnonymousSwitcher>
      <InTenantApp
        multitenant={multitenant}
        specialRoutes={
          multitenant ? (
            <React.Fragment>
              <Route path="/" element={<MultiTenantHome />} />
              <Route path="/_manage/*" element={<ManagementApp />} />
              <Route path="/_connect/*" element={<ConnectComponent />} />
            </React.Fragment>
          ) : (
            <React.Fragment>
              <Route path="/_connect/*" element={<ConnectComponent />} />
            </React.Fragment>
          )
        }
      />
    </AnonymousSwitcher>
  );
};

export default App;
