import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { BusinessesSummary } from "./BusinessesSummary";

export const BusinessesComponent = (props) => {
  let { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute 
          exact 
          path={`${path}`} 
          component={BusinessesSummary} 
        />
      </Switch>
    </React.Fragment>
  );
};
