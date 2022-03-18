import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { useCustomers } from "../../../api-hooks/customersApi";
import { fetchCustomersAsync } from "../../../api/customersApi";
import { useAccessToken } from "../../../api-hooks/token";

export const AsyncSelectCustomer = ({ onChange, placeholder }) => {
  const token = useAccessToken();
  const trackedUsers = useCustomers({});
  const trackedUsersSelectable = trackedUsers.items
    ? trackedUsers.items.map((u) => ({
        label: u.name || u.commonId,
        value: u,
      }))
    : [];

  const loadUsers = (inputValue, callback) => {
    fetchCustomersAsync({
      token,
      searchTerm: inputValue,
    })
      .then((r) =>
        callback(
          r.items.map((x) => ({ value: x, label: x.name || x.commonId }))
        )
      )
      .catch((e) => console.error(e));
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
