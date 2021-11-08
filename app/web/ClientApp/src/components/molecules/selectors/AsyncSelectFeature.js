import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { useFeatures } from "../../../api-hooks/featuresApi";
import { fetchFeaturesAsync } from "../../../api/featuresApi";
import { useAccessToken } from "../../../api-hooks/token";
import { Selector } from "..";

export const AsyncSelectFeature = ({
  onChange,
  placeholder,
  allowNone,
  isMulti,
  defaultCommonIds,
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

  let defaultFeatures = null;
  if (defaultCommonIds && defaultCommonIds.length > 0) {
    if (isMulti) {
      defaultFeatures = featuresSelectable.filter((_) =>
        defaultCommonIds.includes(_.value.commonId)
      );
    } else {
      const defaultId = defaultCommonIds[0];
      defaultFeatures = featuresSelectable.filter(
        (_) => _.value.commonId == defaultId
      )[0];
    }
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

  if (features.loading) {
    return <Selector placeholder="Loading Features..." />;
  }
  return (
    <AsyncSelector
      defaultOptions={featuresSelectable}
      defaultValue={defaultFeatures}
      placeholder={placeholder || "Search for a Feature."}
      cacheOptions
      loadOptions={loadSelectable}
      isMulti={isMulti}
      onChange={onChange}
    />
  );
};
