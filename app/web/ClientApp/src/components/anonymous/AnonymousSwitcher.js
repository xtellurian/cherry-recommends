import React from "react";
import { useAuth } from "../../utility/useAuth";
import { AnonymousLanding } from "./AnonymousLanding";

export const AnonymousSwitcher = ({ children }) => {
  const { isAuthenticated, isLoading } = useAuth();

  if (isAuthenticated) {
    return children;
  } else {
    return <AnonymousLanding isLoading={isLoading} />;
  }
};
