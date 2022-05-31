import React from "react";
import { Navigate } from "react-router-dom";

import { useMemberships } from "./api-hooks/tenantsApi";
import { FunloaderContainer } from "./components/molecules/fullscreen/FunLoader";
import { FunError } from "./components/molecules/fullscreen/FunError";

import "./global-css/cherry.css";

const MultiTenantHome = () => {
  const memberships = useMemberships();
  if (memberships.loading) {
    return <FunloaderContainer />;
  }

  if (memberships.error) {
    return <FunError error={memberships.error.title} />;
  } else if (memberships.length === 0 || memberships.length > 1) {
    return <Navigate to="/_manage" />;
  } else {
    // only 1 memberships
    return <Navigate to={memberships[0].name} />;
  }
};

export default MultiTenantHome;
