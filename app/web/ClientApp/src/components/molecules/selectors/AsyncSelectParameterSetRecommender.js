import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { useParameterSetRecommenders } from "../../../api-hooks/parameterSetRecommendersApi";
import { fetchParameterSetRecommendersAsync } from "../../../api/parameterSetRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";

export const AsyncSelectParameterSetRecommender = ({
  onChange,
  placeholder,
  allowNone,
}) => {
  const token = useAccessToken();
  const recommenders = useParameterSetRecommenders();
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
    fetchParameterSetRecommendersAsync({
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
      placeholder={placeholder || "Search for a Parameter-Set Recommender."}
      cacheOptions
      loadOptions={loadSelectable}
      onChange={onChange}
    />
  );
};
