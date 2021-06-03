import React from "react";
import { useRouteMatch } from "react-router-dom";
import { useTrackedUser } from "../../api-hooks/trackedUserApi";
import { useUserEvents } from "../../api-hooks/eventApi";
import { Spinner } from "../molecules/Spinner";
import { Subtitle, Title } from "../molecules/PageHeadings";
import { JsonView } from "../molecules/JsonView";
import { EventTimelineChart } from "../molecules/EventTimelineChart";

export const TrackedUserDetail = () => {
  const { params } = useRouteMatch();
  const id = params["id"];
  const { trackedUser } = useTrackedUser({ id });
  const { result } = useUserEvents({ commonUserId: trackedUser?.commonUserId });

  if (!trackedUser) {
    return (
      <React.Fragment>
        <Title>Tracked User</Title>
        <Subtitle>...</Subtitle>
        <hr />
        <Spinner />
      </React.Fragment>
    );
  }
  return (
    <React.Fragment>
      <Title>Tracked User</Title>
      <Subtitle>{trackedUser.name || trackedUser.commonUserId}</Subtitle>
      <hr />
      <JsonView data={trackedUser} />
      <hr />
      <div className="mb-5">
        <h4>Events</h4>
        <EventTimelineChart eventResponse={result} />
      </div>
    </React.Fragment>
  );
};
