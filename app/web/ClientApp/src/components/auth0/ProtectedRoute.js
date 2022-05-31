import React from "react";
import { withAuthenticationRequired } from "@auth0/auth0-react";

import { Spinner } from "../molecules/Spinner";

const ProtectedRoute = ({ component }) => {
  const Component = withAuthenticationRequired(component, {
    onRedirecting: () => <Spinner />,
  });

  return <Component />;
};

export default ProtectedRoute;
