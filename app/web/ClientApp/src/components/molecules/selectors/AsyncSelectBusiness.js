import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { useBusinesses } from "../../../api-hooks/businessesApi";
import { fetchBusinessesAsync } from "../../../api/businessesApi";
import { useAccessToken } from "../../../api-hooks/token";

export const AsyncSelectBusiness = ({ onChange, placeholder }) => {
  const token = useAccessToken();
  const businesses = useBusinesses();
  const businessesSelectable = businesses.items
    ? businesses.items.map((u) => ({
        label: u.name || u.commonId,
        value: u,
      }))
    : [];

  const load = (inputValue, callback) => {
    fetchBusinessesAsync({
      token,
      searchTerm: inputValue,
    })
      .then((r) =>
        callback(
          r.items.map((x) => ({ value: x, label: x.name || x.commonId }))
        )
      )
      .catch((e) => console.log(e));
  };

  return (
    <AsyncSelector
      defaultOptions={businessesSelectable}
      placeholder={placeholder || "Search for a business."}
      cacheOptions
      loadOptions={load}
      onChange={onChange}
    />
  );
};
