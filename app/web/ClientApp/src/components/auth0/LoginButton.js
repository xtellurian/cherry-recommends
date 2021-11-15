import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { AsyncButton } from "../molecules";
import { useQuery } from "../../utility/utility";

const LoginButton = ({ isLoading }) => {
  const query = useQuery();
  // const autoSignIn = query.get("autoSignIn");
  const { loginWithRedirect } = useAuth0();
  const [isLoadingInternal, setIsLoadingInternal] = React.useState(false);
  // if (autoSignIn) {
  //   setIsLoadingInternal(true);
  //   setTimeout(() => {
  //     loginWithRedirect()
  //       .then(setIsLoadingInternal(false))
  //       .catch((e) => console.log(e));
  //   }, 500);
  // }
  return (
    <AsyncButton
      loading={isLoading || isLoadingInternal}
      className="btn btn-primary btn-block"
      onClick={() => loginWithRedirect()}
    >
      Log In
    </AsyncButton>
  );
};

export default LoginButton;
