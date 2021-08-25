import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { useFeatures } from "../../../api-hooks/featuresApi";
import { fetchFeaturesAsync } from "../../../api/featuresApi";
import { useAccessToken } from "../../../api-hooks/token";

export const AsyncSelectFeature = ({
  onChange,
  placeholder,
  allowNone,
}) => {
  const token = useAccessToken();
  const features = useFeatures();
  const featuresSelectable = features.items
    ? features.items.map((u) => ({
        label: u.name || u.commonId,
        value: u,
      }))
    : [];
  if (allowNone) {
    featuresSelectable.push({
      value: { commonId: null },
      label: "None",
    });
  }

  const loadSelectable = (inputValue, callback) => {
    fetchFeaturesAsync({
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
      .catch((e) => console.log(e));
  };

  return (
    <AsyncSelector
      defaultOptions={featuresSelectable}
      placeholder={placeholder || "Search for a Feature."}
      cacheOptions
      loadOptions={loadSelectable}
      onChange={onChange}
    />
  );
};
