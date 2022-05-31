import React from "react";
import { Outlet } from "react-router-dom";

import { useAuth } from "../utility/useAuth";
import { useAnalytics } from "./analyticsHooks";

const Identifier = () => {
  const { analytics } = useAnalytics();
  const { user, isAuthenticated } = useAuth();

  React.useEffect(() => {
    if (isAuthenticated) {
      analytics.identify(user.sub, {
        email: user.email,
        cherry_user: true,
      });
    }
  }, [user, isAuthenticated]);

  return <Outlet />;
};

export default Identifier;
