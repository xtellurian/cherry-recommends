import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { AsyncButton } from "../molecules";
import { useQuery } from "../../utility/utility";

const LoginButton = () => {
  const query = useQuery();
  const autoSignIn = query.get("autoSignIn");
  const { loginWithRedirect, isAuthenticated, isLoading } = useAuth0();
  const [aboutToLogin, setAboutToLogin] = React.useState(autoSignIn);

  const signIn = () => {
    console.log("Attempting signin...");
    loginWithRedirect()
      .then(() => {
        console.log("Automatically signed in...");
      })
      .catch((e) => console.log(e))
      .finally(setAboutToLogin(false));
  };
  if (autoSignIn && !isAuthenticated && !isLoading) {
    setTimeout(signIn, 100);
  }
  return (
    <AsyncButton
      loading={isLoading || aboutToLogin}
      className="btn btn-primary btn-block"
      onClick={() => loginWithRedirect()}
    >
      Log In
    </AsyncButton>
  );
};

export default LoginButton;
