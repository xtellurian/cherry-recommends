import React from "react";
import { useAuth } from "../utility/useAuth";
import { Title } from "./molecules/PageHeadings";
import { Spinner } from "./molecules/Spinner";
import { Dashboard } from "./dashboard/Dashboard";
import LoginButton from "./auth0/LoginButton";

const Top = () => {
  return (
    <React.Fragment>
      <Title>Four2 | SignalBox</Title>
      <hr />
    </React.Fragment>
  );
};

const LandingUnauthenticated = () => {
  return (
    <div>
      <div className="text-center">
        <h1 className="display-3">Four 2</h1>
        <p>
          SignalBox is the world's first AI enabled customer intelligence
          platform.
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
        <Top />
        <Spinner />
      </React.Fragment>
    );
  }

  if (isAuthenticated) {
    return (
      <React.Fragment>
        <Title>Dashboard</Title>
        <hr />
        <Dashboard />
      </React.Fragment>
    );
  } else {
    return <LandingUnauthenticated />;
  }
};
