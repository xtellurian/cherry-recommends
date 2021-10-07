import React from "react";
import { useLocation } from "react-router";
import LoginButton from "../auth0/LoginButton";

export const AnonymousLanding = ({ isLoading }) => {

  return (
    <React.Fragment>
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
            <LoginButton isLoading={isLoading} />
          </div>
        </div>
        <div className="mt-2">
          <div className="w-25 m-auto">
            <a href="https://get.four2.ai/sign-up">
              <button className="btn btn-block btn-outline-primary">
                Sign Up
              </button>
            </a>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};
