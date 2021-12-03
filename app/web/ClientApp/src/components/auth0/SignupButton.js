import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { AsyncButton } from "../molecules";
import { useQuery } from "../../utility/utility";

const screen_hint = "signup";
const SignupButton = () => {
  const query = useQuery();
  const autoSignup = query.get("autoSignup");
  const { loginWithRedirect, isAuthenticated, isLoading } = useAuth0();
  const [aboutToSignup, setAboutToSignup] = React.useState(autoSignup);

  const signUp = () => {
    console.log("Attempting signup...");
    loginWithRedirect({ screen_hint })
      .then(() => {
        console.log("Automatically signed in");
      })
      .catch((e) => console.log(e))
      .finally(setAboutToSignup(false));
  };

  if (autoSignup && !isAuthenticated && !isLoading) {
    setTimeout(signUp, 500);
  }
  return (
    <AsyncButton
      loading={isLoading || aboutToSignup}
      className="btn btn-outline-primary btn-block"
      onClick={() => loginWithRedirect({ screen_hint })}
    >
      Sign Up
    </AsyncButton>
  );
};

export default SignupButton;
