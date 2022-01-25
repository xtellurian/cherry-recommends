import React from "react";
import { useAuth } from "../utility/useAuth";
import { Spinner } from "./molecules";
import { Dashboard } from "./dashboard/Dashboard";

export const Home = () => {
  const { isAuthenticated, isLoading } = useAuth();
  if (isLoading) {
    return (
      <React.Fragment>
        <Spinner>Authenticating</Spinner>
      </React.Fragment>
    );
  }

  if (isAuthenticated) {
    return (
      <React.Fragment>
        <Dashboard />
      </React.Fragment>
    );
  } else {
    return <div>Howd we get here...</div>;
  }
};
