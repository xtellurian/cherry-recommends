import React from "react";
import { Link } from "react-router-dom";
import { useTrackedUsers } from "../../api-hooks/trackedUserApi";
import { Spinner } from "./Spinner";
import { EntityRow } from "./layout/EntityRow";

export const TrackedUserListItem = ({ trackedUser }) => {
  return (
    <EntityRow>
      <div className="col">
        <h5>{trackedUser.name || trackedUser.commonId || trackedUser.id}</h5>
      </div>
      <div className="col-3">
        <Link to={`/tracked-users/detail/${trackedUser.id}`}>
          <button className="btn btn-outline-primary btn-block">
            View Customer
          </button>
        </Link>
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
