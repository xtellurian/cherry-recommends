import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { AsyncButton } from "../molecules";
import { useQuery } from "../../utility/utility";
import { useLocation } from "react-router-dom";
import { useAnalytics } from "../../analytics/analyticsHooks";

const LoginButton = () => {
  const query = useQuery();
  const autoSignIn = query.get("autoSignIn");
  const { loginWithRedirect, isAuthenticated, isLoading } = useAuth0();
  const [aboutToLogin, setAboutToLogin] = React.useState(autoSignIn);
  const { pathname } = useLocation();
  const { analytics } = useAnalytics();
  const loginWrapper = () => {
    analytics.track("site:auth0_loginButton_clicked");
    return loginWithRedirect({ appState: { returnTo: pathname } });
  };
  const signIn = () => {
    console.debug("Attempting signin...");
    loginWrapper()
      .then(() => {
        console.info("Automatically signed in...");
      })
      .catch((e) => console.error(e))
      .finally(setAboutToLogin(false));
  };
  if (autoSignIn && !isAuthenticated && !isLoading) {
    setTimeout(signIn, 100);
  }
  return (
    <AsyncButton
      loading={isLoading || aboutToLogin}
      className="btn btn-primary btn-block"
      onClick={() => loginWrapper()}
    >
      Log In
    </AsyncButton>
  );
};

export default LoginButton;
