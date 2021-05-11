import React from "react";
import { useSelectedTrackedUsers } from "../../api-hooks/trackedUserApi";
import { JsonView } from "./JsonView";
import { Spinner } from "./Spinner";
import { ExpandableCard } from "./ExpandableCard";
export const TrackedUserListItem = ({ trackedUser }) => {
  return (
    <ExpandableCard name={trackedUser.name || trackedUser.id}>
      <div>
        <JsonView data={trackedUser} />
      </div>
    </ExpandableCard>
  );
};

export const TrackedUserList = ({ ids }) => {
  const { trackedUsers } = useSelectedTrackedUsers({ids});

  if (!trackedUsers) {
    return <Spinner />;
  }
  if (trackedUsers && trackedUsers.length === 0) {
    return <div className="text-center">The Segment is Empty</div>;
  }
  return trackedUsers.map((u) => (
    <div key={u.id} className="row">
      <div className="col">
        <TrackedUserListItem trackedUser={u} />
      </div>
    </div>
  ));
};
