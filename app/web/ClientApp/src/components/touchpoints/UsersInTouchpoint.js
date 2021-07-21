import React from "react";
import { useParams } from "react-router-dom";
import {
  useTouchpoint,
  useTrackedUsersInTouchpoint,
} from "../../api-hooks/touchpointsApi";
import {
  Title,
  Subtitle,
  Paginator,
  Spinner,
  BackButton,
  EmptyList,
} from "../molecules";
import { TrackedUserListItem } from "../molecules/TrackedUser";

const Top = ({ touchpoint }) => {
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/touchpoints">
        Touchpoints
      </BackButton>
      <Title>Tracked Users in Touchpoint</Title>
      <Subtitle>{touchpoint.name || touchpoint.commonId || "..."}</Subtitle>
    </React.Fragment>
  );
};

export const UsersInTouchpoint = () => {
  const { id } = useParams();
  const touchpoint = useTouchpoint({ id });
  const trackedUsers = useTrackedUsersInTouchpoint({ id });
  return (
    <React.Fragment>
      <Top touchpoint={touchpoint} />
      <hr />
      {trackedUsers.loading && <Spinner />}
      {trackedUsers.items &&
        trackedUsers.items.map((u) => (
          <TrackedUserListItem key={trackedUsers.id} trackedUser={u} />
        ))}

      {trackedUsers.items && trackedUsers.items.length === 0 && (
        <EmptyList>There are no Tracked Users for this touchpoint.</EmptyList>
      )}

      <Paginator {...trackedUsers.pagination} />
    </React.Fragment>
  );
};
