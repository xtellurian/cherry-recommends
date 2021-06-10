import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { UploadTrackedUserComponent } from "./UploadUsers";
import { TrackedUserSummary } from "./TrackedUserSummary";
import { TrackedUserDetail } from "./TrackedUserDetail";
import { CreateUser } from "./CreateUser";
import { TrackedUserTouchpoints } from "./TrackedUserTouchpoints";
import { CreateTouchpointData } from "./CreateTouchpointData";

export const TrackedUsersComponent = (props) => {
  let { path } = useRouteMatch();
  return (
    <React.Fragment>
      <div>
        <Switch>
          <AuthorizeRoute
            exact
            path={`${path}`}
            component={TrackedUserSummary}
          />
          <AuthorizeRoute
            exact
            path={`${path}/upload`}
            component={UploadTrackedUserComponent}
          />
          <AuthorizeRoute
            exact
            path={`${path}/create`}
            component={CreateUser}
          />
          <AuthorizeRoute
            exact
            path={`${path}/detail/:id`}
            component={TrackedUserDetail}
          />
          <AuthorizeRoute
            exact
            path={`${path}/touchpoints/:id`}
            component={TrackedUserTouchpoints}
          />
          <AuthorizeRoute
            exact
            path={`${path}/create-touchpoint-data/:id`}
            component={CreateTouchpointData}
          />
        </Switch>
      </div>
    </React.Fragment>
  );
};
