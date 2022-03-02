import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { BusinessesSummary } from "./BusinessesSummary";
import { BusinessDetail } from "./BusinessDetail";
import { CreateBusiness } from "./CreateBusiness";
import { EditBusinessProperties } from "./EditBusinessProperties";

export const BusinessesComponent = (props) => {
  let { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={BusinessesSummary} />
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
      </Switch>
    </React.Fragment>
  );
};
