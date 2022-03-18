import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { Spinner } from "../Spinner";
import { usePromotions } from "../../../api-hooks/promotionsApi";
import { fetchPromotionsAsync } from "../../../api/promotionsApi";
import { useAccessToken } from "../../../api-hooks/token";

export const AsyncSelectItem = ({
  onChange,
  placeholder,
  defaultIds,
  allowNone,
  isMulti,
}) => {
  const token = useAccessToken();
  const items = usePromotions();
  const itemsSelectable = items.items
    ? items.items.map((u) => ({
        label: u.name || u.commonId,
        value: u,
      }))
    : [];
  if (allowNone) {
    itemsSelectable.push({
      value: { commonId: null },
      label: "None",
    });
  }
  let defaultItems = null;
  if (defaultIds && defaultIds.length > 0) {
    if (isMulti) {
      defaultItems = itemsSelectable.filter((_) =>
        defaultIds.includes(_.value.id)
      );
    } else {
      throw new Error("single selector not enabled");
    }
  }

  const loadSelectable = (inputValue, callback) => {
    console.debug(inputValue);
    fetchPromotionsAsync({
      token,
      searchTerm: inputValue,
    })
      .then((r) => {
        console.debug(r);
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

  if (items.loading) {
    return <Spinner />;
  }
  return (
    <AsyncSelector
      defaultValue={defaultItems}
      defaultOptions={itemsSelectable}
      placeholder={placeholder || "Search for a Promotion."}
      cacheOptions
      isMulti={isMulti}
      loadOptions={loadSelectable}
      onChange={onChange}
    />
  );
};
