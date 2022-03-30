import React from "react";
import LoginButton from "../auth0/LoginButton";
import SignupButton from "../auth0/SignupButton";

export const AnonymousLanding = () => {
  return (
    <React.Fragment>
      <div>
        <div className="text-center mt-4">
          <h1 className="display-3">Cherry Recommends</h1>
          <p>Cherry is an all-in-one promotion management platform.</p>
        </div>
        <div className="mt-5">
          <div className="w-25 m-auto">
            <LoginButton />
          </div>
        </div>
        <div className="mt-1">
          <div className="w-25 m-auto">
            <SignupButton />
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};
