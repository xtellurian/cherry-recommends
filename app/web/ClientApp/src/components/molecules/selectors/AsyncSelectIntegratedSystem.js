import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { useIntegratedSystems } from "../../../api-hooks/integratedSystemsApi";
import { fetchIntegratedSystemsAsync } from "../../../api/integratedSystemsApi";
import { useAccessToken } from "../../../api-hooks/token";

export const AsyncSelectIntegratedSystem = ({
  value,
  onChange,
  placeholder,
  allowNone,
  systemType,
}) => {
  const token = useAccessToken();
  const entities = useIntegratedSystems({ systemType });
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
      .catch((e) => console.error(e));
  };

  const newValue =
    value !== undefined
      ? entitiesSelectable?.find((el) => el?.value?.id === value?.id) || null
      : undefined;

  return (
    <AsyncSelector
      value={newValue}
      defaultOptions={entitiesSelectable}
      placeholder={placeholder || "Search..."}
      cacheOptions
      loadOptions={loadSelectable}
      onChange={onChange}
    />
  );
};
