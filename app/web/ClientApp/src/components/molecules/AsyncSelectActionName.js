import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { useUniqueActionNames } from "../../api-hooks/actionsApi";
import { fetchUniqueActionNamesAsync } from "../../api/actionsApi";
import { useAccessToken } from "../../api-hooks/token";

function onlyUnique(value, index, self) {
  return self.indexOf(value) === index;
}

export const AsyncSelectActionName = ({ onChange, placeholder }) => {
  const token = useAccessToken();
  const actionNames = useUniqueActionNames();
  const actionNamesSelectable = actionNames.items
    ? actionNames.items
        .map((_) => _.actionName)
        .filter(onlyUnique)
        .map((x) => ({ value: x, label: x }))
    : [];

  const loadActionNames = (term, callback) => {
    fetchUniqueActionNamesAsync({
      token,
      term,
    })
      .then((r) =>
        callback(
          r.items
            .map((_) => _.actionName)
            .filter(onlyUnique)
            .map((x) => ({ value: x, label: x }))
        )
      )
      .catch((e) => console.log(e));
  };

  return (
    <AsyncSelector
      defaultOptions={actionNamesSelectable}
      placeholder={placeholder || "Search for an action."}
      cacheOptions
      loadOptions={loadActionNames}
      onChange={onChange}
    />
  );
};
