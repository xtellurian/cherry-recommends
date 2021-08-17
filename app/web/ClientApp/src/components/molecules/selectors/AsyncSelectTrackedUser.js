import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { useTrackedUsers } from "../../../api-hooks/trackedUserApi";
import { fetchTrackedUsersAsync } from "../../../api/trackedUsersApi";
import { useAccessToken } from "../../../api-hooks/token";

export const AsyncSelectTrackedUser = ({ onChange, placeholder }) => {
  const token = useAccessToken();
  const trackedUsers = useTrackedUsers({});
  const trackedUsersSelectable = trackedUsers.items
    ? trackedUsers.items.map((u) => ({
        label: u.name || u.commonId,
        value: u,
      }))
    : [];

  const loadUsers = (inputValue, callback) => {
    fetchTrackedUsersAsync({
      token,
      searchTerm: inputValue,
    })
      .then((r) =>
        callback(
          r.items.map((x) => ({ value: x, label: x.name || x.commonId }))
        )
      )
      .catch((e) => console.log(e));
  };

  return (
    <AsyncSelector
      defaultOptions={trackedUsersSelectable}
      placeholder={placeholder || "Search for a user."}
      cacheOptions
      loadOptions={loadUsers}
      onChange={onChange}
    />
  );
};
