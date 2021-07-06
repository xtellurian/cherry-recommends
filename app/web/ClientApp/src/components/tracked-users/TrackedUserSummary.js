import React from "react";
import { Link } from "react-router-dom";
import { useTrackedUsers } from "../../api-hooks/trackedUserApi";
import { Title, Spinner, Paginator } from "../molecules";
import { SearchBox } from "../molecules/SearchBox";
import { TrackedUserListItem } from "../molecules/TrackedUser";
import { EmptyList } from "../molecules/EmptyList";

const CreateButton = () => {
  return (
    <Link to="/tracked-users/create">
      <button className="btn btn-primary">Create a User</button>
    </Link>
  );
};
export const TrackedUserSummary = () => {
  const [searchTerm, setSearchTerm] = React.useState("");
  const trackedUsers = useTrackedUsers({ searchTerm });

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
      {trackedUsers.loading && <Spinner />}
      <SearchBox onSearch={setSearchTerm} />
      {trackedUsers.items && trackedUsers.items.length === 0 && (
        <EmptyList>
          There are no tracked users.
          <div className="mt-3">
            <CreateButton />
          </div>
        </EmptyList>
      )}
      <div>
        {trackedUsers.items &&
          trackedUsers.items.map((u) => (
            <TrackedUserListItem
              key={u.id}
              trackedUser={u}
              // events={eventDic && u.id in eventDic ? eventDic[u.id] : []}
            />
          ))}
      </div>
      <Paginator {...trackedUsers.pagination} />
    </div>
  );
};
