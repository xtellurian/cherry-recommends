import React from "react";
import { Link } from "react-router-dom";
import { useTrackedUsers } from "../../api-hooks/trackedUserApi";
import { Title } from "../molecules/PageHeadings";
import { Spinner } from "../molecules/Spinner";
import { Paginator } from "../molecules/Paginator";
import { TrackedUserListItem } from "../molecules/TrackedUser";

const CreateButton = () => {
  return (
    <Link to="/tracked-users/create">
      <button className="btn btn-primary">Create a User</button>
    </Link>
  );
};
export const TrackedUserSummary = () => {
  const { result } = useTrackedUsers();

  if (!result) {
    return <Spinner />;
  }

  const trackedUsers = result.items;

  return (
    <div>
      <div className="float-right">
        <CreateButton />
        <Link to="tracked-users/upload">
          <button className="btn btn-outline-primary ml-1">Upload CSV</button>
        </Link>
      </div>
      <Title>Tracked Users</Title>
      <hr />
      {result.totalItemCount === 0 && (
        <div className="text-center">
          There are no tracked users.
          <div className="mt-3">
            <CreateButton />
          </div>
        </div>
      )}
      <div>
        {trackedUsers.map((u) => (
          <TrackedUserListItem
            key={u.id}
            trackedUser={u}
            // events={eventDic && u.id in eventDic ? eventDic[u.id] : []}
          />
        ))}
      </div>
      <Paginator {...result.pagination} />
    </div>
  );
};
