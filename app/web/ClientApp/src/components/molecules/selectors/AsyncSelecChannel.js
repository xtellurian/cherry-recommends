import React from "react";
import { AsyncSelector } from "./AsyncSelect";
import { fetchChannelsAsync } from "../../../api/channelsApi";
import { useAccessToken } from "../../../api-hooks/token";
import { useChannels } from "../../../api-hooks/channelsApi";

export const AsyncSelectChannel = ({ onChange, placeholder }) => {
  const token = useAccessToken();
  const channels = useChannels();
  const channelsSelectable = channels.items
    ? channels.items.map((u) => ({
        label: u.name,
        value: u,
      }))
    : [];

  const load = (inputValue, callback) => {
    fetchChannelsAsync({
      token,
      searchTerm: inputValue,
    })
      .then((r) =>
        callback(
          r.items.map((x) => ({
            value: x,
            label: x.name,
            endpoint: x.endpoint,
          }))
        )
      )
      .catch((e) => console.error(e));
  };

  return (
    <AsyncSelector
      defaultOptions={channelsSelectable}
      placeholder={placeholder || "Search for a channel."}
      cacheOptions
      loadOptions={load}
      onChange={onChange}
    />
  );
};
