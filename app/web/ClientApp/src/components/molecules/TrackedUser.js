import React from "react";
import { Link } from "react-router-dom";
import { useTrackedUsers } from "../../api-hooks/trackedUserApi";
import { JsonView } from "./JsonView";
import { Spinner } from "./Spinner";
import { ExpandableCard } from "./ExpandableCard";
import { EntityRow } from "./layout/EntityRow";

export const TrackedUserListItem = ({ trackedUser }) => {
  return (
    <EntityRow>
      <div className="col">
        <div className="mb-3">
          <Link
            to={`/tracked-users/detail/${trackedUser.id}`}
            className="float-right"
          >
            <button className="btn btn-primary btn-sm">Detail</button>
          </Link>
          <div>
            {trackedUser.name || trackedUser.commonId || trackedUser.id}
          </div>
        </div>
        <div className="mt-2">
          <ExpandableCard label="Properties">
            <JsonView data={trackedUser} />
          </ExpandableCard>
        </div>
      </div>
    </EntityRow>
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
