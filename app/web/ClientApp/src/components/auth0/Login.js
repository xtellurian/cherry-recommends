import React from "react";
import { withAuthenticationRequired } from "@auth0/auth0-react";
// import authService from './AuthorizeService';
// import { AuthenticationResultStatus } from './AuthorizeService';
import {
  LoginActions,
  QueryParameterNames,
  ApplicationPaths,
} from "./ApiAuthorizationConstants";

// The main responsibility of this component is to handle the user's login process.
// This is the starting point for the login process. Any component that needs to authenticate
// a user can simply perform a redirect to this component with a returnUrl query parameter and
// let the component perform the login and return back to the return url.
const LoginPage = () => {
  return <div>Redirecting you to login</div>;
};

export default withAuthenticationRequired(LoginPage);
