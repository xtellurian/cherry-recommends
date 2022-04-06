import React from "react";
import AuthorizeRoute from "./components/auth0/ProtectedRoute";
import "./global-css/cherry.css";

import { useMemberships } from "./api-hooks/tenantsApi";
import { FunloaderContainer } from "./components/molecules/fullscreen/FunLoader";
import { FunError } from "./components/molecules/fullscreen/FunError";
import { Redirect } from "react-router-dom";

const MultiTenantHome = () => {
  const memberships = useMemberships();
  if (memberships.loading) {
    return <FunloaderContainer />;
  }

  if (memberships.error) {
    return <FunError error={memberships.error.title} />;
  } else if (memberships.length === 0 || memberships.length > 1) {
    return <Redirect to="/_manage" />;
  } else {
    // only 1 memberships
    return <Redirect to={memberships[0].name} />;
  }
};

export default MultiTenantHome;
