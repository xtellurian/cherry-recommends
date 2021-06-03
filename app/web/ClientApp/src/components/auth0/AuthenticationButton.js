import React from "react";

import LoginButton from "./LoginButton";
import LogoutButton from "./LogoutButton";

import { useAuth } from "../../utility/useAuth";

const AuthenticationButton = () => {
  const { isAuthenticated, isLoading } = useAuth();
  if (isLoading) {
    return (
      <div className="d-flex justify-content-center">
        <div className="spinner-grow text-info" role="status">
          <span className="sr-only">Logging in...</span>
        </div>
      </div>
    );
  }
  return isAuthenticated ? <LogoutButton /> : <LoginButton />;
};

export default AuthenticationButton;
