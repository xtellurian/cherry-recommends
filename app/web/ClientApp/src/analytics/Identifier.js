import React from "react";
import { useAuth } from "../utility/useAuth";
import { useAnalytics } from "./analyticsHooks";

const Identifier = ({ children }) => {
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

  return <>{children}</>;
};

export default Identifier;
