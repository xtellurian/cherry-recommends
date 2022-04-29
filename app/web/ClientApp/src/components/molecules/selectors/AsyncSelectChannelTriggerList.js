import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { useAccessToken } from "../../../api-hooks/token";
import { useKlaviyoLists } from "../../../api-hooks/klaviyoApi";
import { fetchKlaviyoListsAsync } from "../../../api/klaviyoApi";
import { useTenantName } from "../../tenants/PathTenantProvider";

export const AsyncSelectChannelTriggerList = ({
  value,
  onChange,
  placeholder,
  integratedSystemId,
}) => {
  const token = useAccessToken();
  const { tenantName } = useTenantName();
  const lists = useKlaviyoLists({ id: integratedSystemId });
  const entitiesSelectable =
    lists.length > 0
      ? lists?.map((u) => ({
          label: u.list_name,
          value: u,
        }))
      : [];

  const loadSelectable = (inputValue, callback) => {
    fetchKlaviyoListsAsync({
      token,
      tenant: tenantName,
      id: integratedSystemId,
      searchTerm: inputValue,
    })
      .then((r) => {
        const selectable = r.map((x) => ({
          value: x,
          label: x.list_name,
        }));
        callback(selectable);
      })
      .catch((e) => console.error(e));
  };

  const selectedValue =
    value !== undefined
      ? entitiesSelectable?.find(
          (el) => el?.value?.list_id === value?.list_id
        ) || null
      : undefined;

  return (
    <AsyncSelector
      value={selectedValue}
      defaultOptions={entitiesSelectable}
      placeholder={placeholder || "Select email channel trigger list"}
      cacheOptions
      loadOptions={loadSelectable}
      onChange={onChange}
    />
  );
};
