import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import { TenantsComponent } from "./components/tenants/TenantsComponent";
import AuthorizeRoute from "./components/auth0/ProtectedRoute";
import "./global-css/cherry.css";

const ManagementApp = () => {
  const { path } = useRouteMatch();
  return (
    <Switch>
      <AuthorizeRoute path={path} component={TenantsComponent} />
    </Switch>
  );
};

export default ManagementApp;
