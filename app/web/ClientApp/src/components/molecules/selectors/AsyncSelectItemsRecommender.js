import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { Spinner } from "../Spinner";
import { usePromotionsRecommenders } from "../../../api-hooks/promotionsRecommendersApi";
import { fetchPromotionsRecommendersAsync } from "../../../api/promotionsRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";

export const AsyncSelectItemsRecommender = ({
  onChange,
  placeholder,
  defaultIds,
  allowNone,
  isMulti,
}) => {
  const token = useAccessToken();
  const recommenders = usePromotionsRecommenders();
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
  let defaultRecommenders = null;
  if (defaultIds && defaultIds.length > 0) {
    if (isMulti) {
      defaultRecommenders = recommendersSelectable.filter((_) =>
        defaultIds.includes(_.value.id)
      );
    } else {
      throw new Error("single selector not enabled");
    }
  }

  const loadSelectable = (inputValue, callback) => {
    fetchPromotionsRecommendersAsync({
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

  if (recommenders.loading) {
    return <Spinner />;
  }
  return (
    <AsyncSelector
      defaultValue={defaultRecommenders}
      defaultOptions={recommendersSelectable}
      placeholder={placeholder || "Search for a Promotion Recommender."}
      cacheOptions
      isMulti={isMulti}
      loadOptions={loadSelectable}
      onChange={onChange}
    />
  );
};
