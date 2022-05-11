import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { UploadTrackedUserComponent } from "./UploadUsers";
import { CustomersSummary } from "./CustomersSummary";
import { CustomerDetail } from "./CustomerDetail";
import { CreateCustomer } from "./CreateCustomer";
import { LinkToIntegratedSystem } from "./LinkToIntegratedSystem";
import { EditProperties } from "./EditProperties";
import { CreateEvent } from "./CreateEvent";
import Metrics from "./Metrics";
import { ErrorBoundary } from "../molecules/ErrorBoundary";

export const CustomersComponent = (props) => {
  let { path } = useRouteMatch();
  return (
    <React.Fragment>
      <div>
        <ErrorBoundary>
          <Switch>
            <AuthorizeRoute
              exact
              path={`${path}`}
              component={CustomersSummary}
            />
            <AuthorizeRoute
              exact
              path={`${path}/upload`}
              component={UploadTrackedUserComponent}
            />
            <AuthorizeRoute
              exact
              path={`${path}/create`}
              component={CreateCustomer}
            />
            <AuthorizeRoute
              exact
              path={`${path}/detail/:id`}
              component={CustomerDetail}
            />
            <AuthorizeRoute
              exact
              path={`${path}/link-to-integrated-system/:id`}
              component={LinkToIntegratedSystem}
            />
            <AuthorizeRoute
              exact
              path={`${path}/metrics/:id`}
              component={Metrics}
            />
            <AuthorizeRoute
              exact
              path={`${path}/edit-properties/:id`}
              component={EditProperties}
            />
            <AuthorizeRoute
              exact
              path={`${path}/create-event/:id`}
              component={CreateEvent}
            />
          </Switch>
        </ErrorBoundary>
      </div>
    </React.Fragment>
  );
};
