import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { AsyncButton } from "../molecules";
import { useQuery } from "../../utility/utility";

const screen_hint = "signup";
const SignupButton = () => {
  const query = useQuery();
  const autoSignup = query.get("autoSignup");
  const { loginWithRedirect, isAuthenticated, isLoading } = useAuth0();

  if (autoSignup && !isAuthenticated) {
    setTimeout(() => {
      loginWithRedirect({ screen_hint })
        .then(() => {
          console.log("Automatically signed in");
        })
        .catch((e) => console.log(e));
    }, 5000);
  }
  return (
    <AsyncButton
      loading={isLoading}
      className="btn btn-outline-primary btn-block"
      onClick={() => loginWithRedirect({ screen_hint })}
    >
      Sign Up
    </AsyncButton>
  );
};

export default SignupButton;
