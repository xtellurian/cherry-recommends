import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { useIntegratedSystems } from "../../../api-hooks/integratedSystemsApi";
import { fetchIntegratedSystemsAsync } from "../../../api/integratedSystemsApi";
import { useAccessToken } from "../../../api-hooks/token";

export const AsyncSelectIntegratedSystem = ({
  onChange,
  placeholder,
  allowNone,
}) => {
  const token = useAccessToken();
  const entities = useIntegratedSystems();
  const entitiesSelectable = entities.items
    ? entities.items.map((u) => ({
        label: u.name || u.commonId,
        value: u,
      }))
    : [];

  const loadSelectable = (inputValue, callback) => {
    fetchIntegratedSystemsAsync({
      token,
      searchTerm: inputValue,
    })
      .then((r) => {
        const selectable = r.items.map((x) => ({
          value: x,
          label: x.name || x.commonId,
        }));
        callback(selectable);
      })
      .catch((e) => console.log(e));
  };

  return (
    <AsyncSelector
      defaultOptions={entitiesSelectable}
      placeholder={placeholder || "Search..."}
      cacheOptions
      loadOptions={loadSelectable}
      onChange={onChange}
    />
  );
};
