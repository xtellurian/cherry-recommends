import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { useAnalytics } from "../../analytics/analyticsHooks";

const LogoutButton = () => {
  const { logout } = useAuth0();
  const { analytics } = useAnalytics();
  return (
    <button
      className="btn btn-outline-danger btn-sm"
      onClick={() => {
        analytics.reset();
        logout({
          returnTo: window.location.origin,
        });
      }}
    >
      Log Out
    </button>
  );
};

export default LogoutButton;
