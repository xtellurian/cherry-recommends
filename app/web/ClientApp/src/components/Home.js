import React from "react";
import { useAuth } from "../utility/useAuth";
import { Spinner } from "./molecules";
import { Dashboard } from "./dashboard/Dashboard";
import LoginButton from "./auth0/LoginButton";

const LandingUnauthenticated = () => {
  return (
    <div>
      <div className="text-center">
        <h1 className="display-3">Four 2</h1>
        <p>
          SignalBox is the leading Customer Recommendation Platform for
          subscription companies.
        </p>
      </div>
      <div className="mt-5">
        <div className="w-25 m-auto">
          <LoginButton />
        </div>
      </div>
    </div>
  );
};
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
    return <LandingUnauthenticated />;
  }
};
