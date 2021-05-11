import React from "react";
import { Link, Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { CreateSegment } from "./CreateSegment";
import { SegmentList } from "./SegmentList";
import { SegmentMembers } from "./SegmentMembers";
import { SegmentRules } from "./SegmentRules";
import { CreateRule } from "./CreateRule";
import { CreateButton } from "../molecules/CreateButton";

const SegmentsHome = () => {
  let { path } = useRouteMatch();
  return (
    <div>
      <CreateButton to={`${path}/create`} className="float-right">
        Create New Segment
      </CreateButton>
      <SegmentList />
    </div>
  );
};

export const SegmentsComponent = () => {
  let { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={SegmentsHome} />
        <AuthorizeRoute
          exact
          path={`${path}/create`}
          component={CreateSegment}
        />
        <AuthorizeRoute
          exact
          path={`${path}/:id/members`}
          component={SegmentMembers}
        />
        <AuthorizeRoute
          exact
          path={`${path}/:id/rules`}
          component={SegmentRules}
        />
        <AuthorizeRoute
          exact
          path={`${path}/create-rule`}
          component={CreateRule}
        />
      </Switch>
    </React.Fragment>
  );
};
