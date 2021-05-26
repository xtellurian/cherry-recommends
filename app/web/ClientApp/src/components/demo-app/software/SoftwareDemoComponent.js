import React from "react";
import { Switch, Link } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { CancelSoftwareSubscription } from "./CancelSoftwareSubscription";
import { LandingPage } from "./Landing";
import { Results } from "./Results";
import { ChoosePersona } from "./ChoosePersona";

const bottomStyle = {
  position: "absolute",
  left: 0,
  bottom: 0,
  right: 0,
  marginRight: "20px",
  marginBottom: "20px",
};
export const SoftwareDemoComponent = () => {
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path="/demo/software" component={ChoosePersona} />
        <AuthorizeRoute
          exact
          path="/demo/software/landing"
          component={LandingPage}
        />
        <AuthorizeRoute
          exact
          path="/demo/software/cancel"
          component={CancelSoftwareSubscription}
        />
        <AuthorizeRoute
          exact
          path="/demo/software/results"
          component={Results}
        />
      </Switch>
      <div style={bottomStyle}>
        <Link to="/demo/software/results" className="float-right">
          <button className="btn btn-outline-primary">Results</button>
        </Link>
        <Link to="/demo/software/" className="ml-3">
          <button className="btn btn-outline-primary">Demo</button>
        </Link>
      </div>
    </React.Fragment>
  );
};
