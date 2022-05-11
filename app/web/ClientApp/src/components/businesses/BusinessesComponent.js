import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { BusinessesSummary } from "./BusinessesSummary";
import { BusinessDetail } from "./BusinessDetail";
import { CreateBusiness } from "./CreateBusiness";
import { EditBusinessProperties } from "./EditBusinessProperties";
import { BusinessMetrics } from "./BusinessMetrics";
import { ErrorBoundary } from "../molecules/ErrorBoundary";

export const BusinessesComponent = (props) => {
  let { path } = useRouteMatch();
  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute
            exact
            path={`${path}`}
            component={BusinessesSummary}
          />
          <AuthorizeRoute
            exact
            path={`${path}/detail/:id`}
            component={BusinessDetail}
          />
          <AuthorizeRoute
            exact
            path={`${path}/create`}
            component={CreateBusiness}
          />
          <AuthorizeRoute
            exact
            path={`${path}/edit-properties/:id`}
            component={EditBusinessProperties}
          />
          <AuthorizeRoute
            exact
            path={`${path}/metrics/:id`}
            component={BusinessMetrics}
          />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};
