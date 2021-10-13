import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { Spinner } from "../Spinner";
import { useItems } from "../../../api-hooks/recommendableItemsApi";
import { fetchItemsAsync } from "../../../api/recommendableItemsApi";
import { useAccessToken } from "../../../api-hooks/token";

export const AsyncSelectItem = ({
  onChange,
  placeholder,
  defaultIds,
  allowNone,
  isMulti,
}) => {
  const token = useAccessToken();
  const items = useItems();
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
    console.log(inputValue);
    fetchItemsAsync({
      token,
      searchTerm: inputValue,
    })
      .then((r) => {
          console.log(r)
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

  if (items.loading) {
    return <Spinner />;
  }
  return (
    <AsyncSelector
      defaultValue={defaultItems}
      defaultOptions={itemsSelectable}
      placeholder={placeholder || "Search for an Item."}
      cacheOptions
      isMulti={isMulti}
      loadOptions={loadSelectable}
      onChange={onChange}
    />
  );
};