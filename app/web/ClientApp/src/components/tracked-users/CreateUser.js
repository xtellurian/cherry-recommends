import React from "react";
import { createSingleUser } from "../../api/trackedUsersApi";
import { useAccessToken } from "../../api-hooks/token";
import { Title } from "../molecules/PageHeadings";

export const CreateUser = () => {
  const [newUser, setNewUser] = React.useState({ commonUserId: "", name: "" });
  const token = useAccessToken();
  return (
    <React.Fragment>
      <div>
        <Title>Track a new User</Title>
        <hr />
        <label className="form-label">
          Just enter their unique identifier from your system.
        </label>
        <div className="input-group">
          <input
            type="text"
            className="form-control"
            placeholder="Name"
            value={newUser.name}
            onChange={(e) =>
              setNewUser({
                ...newUser,
                name: e.target.value,
              })
            }
          />
          <input
            type="text"
            className="form-control"
            placeholder="User ID"
            value={newUser.commonUserId}
            onChange={(e) =>
              setNewUser({
                ...newUser,
                commonUserId: e.target.value,
              })
            }
          />
          <button
            className="btn btn-primary"
            onClick={() => {
              createSingleUser({
                success: () => alert("created a user!"),
                error: (e) => alert(e),
                user: newUser,
                token
              });
            }}
          >
            Create
          </button>
        </div>
      </div>
    </React.Fragment>
  );
};
