import React from "react";
import { useTrackedUsers } from "../../api-hooks/trackedUserApi";
import { JsonView } from "./JsonView";
import { Spinner } from "./Spinner";
import { ExpandableCard } from "./ExpandableCard";
import { Link } from "react-router-dom";
export const TrackedUserListItem = ({ trackedUser }) => {
  return (
    <ExpandableCard label={trackedUser.name || trackedUser.id}>
      <div>
        <Link
          to={`/tracked-users/detail/${trackedUser.id}`}
          className="float-right"
        >
          <button className="btn btn-primary">Detail</button>
        </Link>
        <JsonView data={trackedUser} />
      </div>
    </ExpandableCard>
  );
};

export const TrackedUserList = ({ ids }) => {
  console.log("WARNING: THIS ISNt IMPLEMNTED");
  const trackedUsers = useTrackedUsers({});

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
