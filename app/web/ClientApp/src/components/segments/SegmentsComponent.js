import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { CreateSegment } from "./CreateSegment";
import { SegmentSummary } from "./SegmentSummary";
import { SegmentDetail } from "./SegmentDetail";
import { CreateButtonClassic } from "../molecules/CreateButton";
import { useFeatureFlag } from "../launch-darkly/hooks";

const SegmentsHome = () => {
  let { path } = useRouteMatch();
  return (
    <div>
      <CreateButtonClassic to={`${path}/create`} className="float-right">
        Create New Segment
      </CreateButtonClassic>
      <SegmentSummary />
    </div>
  );
};

export const SegmentsComponent = () => {
  let { path } = useRouteMatch();
  const flag = useFeatureFlag("segments", true);

  if (!flag) {
    return null;
  }

  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={SegmentsHome} />
        <AuthorizeRoute
          exact
          path={`${path}/detail/:id`}
          component={SegmentDetail}
        />
        <AuthorizeRoute
          exact
          path={`${path}/create`}
          component={CreateSegment}
        />
      </Switch>
    </React.Fragment>
  );
};
