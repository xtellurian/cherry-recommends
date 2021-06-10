import React from "react";
import { useRouteMatch } from "react-router-dom";
import {
  useTrackedUserTouchpoints,
  useTrackedUserTouchpointValues,
} from "../../api-hooks/touchpointsApi";
import { useTrackedUser } from "../../api-hooks/trackedUserApi";
import { Subtitle, Title } from "../molecules/PageHeadings";
import { Spinner } from "../molecules/Spinner";
import { EmptyList } from "../molecules/EmptyList";
import { CreateButton } from "../molecules/CreateButton";
import { BackButton } from "../molecules/BackButton";
import { ExpandableCard } from "../molecules/ExpandableCard";
import { JsonView } from "../molecules/JsonView";

const Top = () => {
  return (
    <React.Fragment>
      <Title>Tracked User Touchpoints</Title>
    </React.Fragment>
  );
};

const TouchpointRow = ({ trackedUser, touchpointMeta }) => {
  const values = useTrackedUserTouchpointValues({
    id: trackedUser.id,
    touchpointCommonId: touchpointMeta.commonId,
  });
  return (
    <React.Fragment>
      <ExpandableCard name={touchpointMeta.name || touchpointMeta.commonId}>
        {(!values || values.loading) && <Spinner />}
        {values && !values.loading && <JsonView data={values} />}
      </ExpandableCard>
    </React.Fragment>
  );
};

export const TrackedUserTouchpoints = () => {
  const { params } = useRouteMatch();
  const id = params["id"];
  const trackedUser = useTrackedUser({ id });
  const touchpoints = useTrackedUserTouchpoints({ id });
  if (!touchpoints || touchpoints.loading) {
    return (
      <React.Fragment>
        <BackButton
          className="float-right"
          to={`/tracked-users/detail/${trackedUser.id}`}
        >
          Tracked User
        </BackButton>
        <Top />
        <Subtitle>
          {trackedUser.loading ? "..." : trackedUser.commonId}
        </Subtitle>
        <Spinner>Loading Touchpoints</Spinner>
      </React.Fragment>
    );
  }
  return (
    <React.Fragment>
      <CreateButton
        className="ml-1 float-right"
        to={`/tracked-users/create-touchpoint-data/${trackedUser.id}`}
      >
        Create Touchpoint Data
      </CreateButton>
      <BackButton
        className="float-right"
        to={`/tracked-users/detail/${trackedUser.id}`}
      >
        Tracked User
      </BackButton>
      <Top />
      <Subtitle>
        {trackedUser.loading ? "..." : trackedUser.name || trackedUser.commonId}
      </Subtitle>
      <hr />
      {touchpoints && touchpoints.length === 0 && (
        <EmptyList>There are no touchpoints for this user.</EmptyList>
      )}
      {touchpoints.map((t) => (
        <TouchpointRow
          key={t.id}
          trackedUser={trackedUser}
          touchpointMeta={t}
        />
      ))}
    </React.Fragment>
  );
};
