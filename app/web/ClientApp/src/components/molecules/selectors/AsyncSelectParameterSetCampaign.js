import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { useParameterSetCampaigns } from "../../../api-hooks/parameterSetCampaignsApi";
import { fetchParameterSetCampaignsAsync } from "../../../api/parameterSetCampaignsApi";
import { useAccessToken } from "../../../api-hooks/token";

export const AsyncSelectParameterSetCampaign = ({
  onChange,
  placeholder,
  allowNone,
}) => {
  const token = useAccessToken();
  const recommenders = useParameterSetCampaigns();
  const recommendersSelectable = recommenders.items
    ? recommenders.items.map((u) => ({
        label: u.name || u.commonId,
        value: u,
      }))
    : [];
  if (allowNone) {
    recommendersSelectable.push({
      value: { commonId: null },
      label: "None",
    });
  }

  const loadSelectable = (inputValue, callback) => {
    fetchParameterSetCampaignsAsync({
      token,
      searchTerm: inputValue,
    })
      .then((r) => {
        const selectable = r.items.map((x) => ({
          value: x,
          label: x.name || x.commonId,
        }));
        if (allowNone) {
          selectable.push({
            value: { commonId: null },
            label: "None",
          });
        }
        callback(selectable);
      })
      .catch((e) => console.error(e));
  };

  return (
    <AsyncSelector
      defaultOptions={recommendersSelectable}
      placeholder={placeholder || "Search for a Parameter-Set Campaign."}
      cacheOptions
      loadOptions={loadSelectable}
      onChange={onChange}
    />
  );
};
