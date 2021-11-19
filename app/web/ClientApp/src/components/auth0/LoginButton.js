import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { AsyncButton } from "../molecules";
import { useQuery } from "../../utility/utility";

const LoginButton = () => {
  const query = useQuery();
  const autoSignIn = query.get("autoSignIn");
  const { loginWithRedirect, isAuthenticated, isLoading } = useAuth0();

  if (autoSignIn && !isAuthenticated) {
    setTimeout(() => {
      loginWithRedirect()
        .then(() => {
          console.log("Automatically signed in");
        })
        .catch((e) => console.log(e));
    }, 500);
  }
  return (
    <AsyncButton
      loading={isLoading}
      className="btn btn-primary btn-block"
      onClick={() => loginWithRedirect()}
    >
      Log In
    </AsyncButton>
  );
};

export default LoginButton;
