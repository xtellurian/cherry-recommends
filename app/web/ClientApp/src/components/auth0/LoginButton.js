import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { AsyncButton } from "../molecules";

const LoginButton = ({ isLoading }) => {
  const { loginWithRedirect } = useAuth0();
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
