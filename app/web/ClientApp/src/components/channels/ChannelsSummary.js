import React from "react";
import { CreateButtonClassic } from "../molecules/CreateButton";
import { useChannels } from "../../api-hooks/channelsApi";
import { Title, Spinner, Paginator, ErrorCard } from "../molecules";
import { EmptyList } from "../molecules";
import { ChannelRow } from "./ChannelRow";

export const ChannelsSummary = () => {
  const channels = useChannels();

  return (
    <div>
      <div className="float-right">
        <CreateButtonClassic to="/channels/create">
          Create New
        </CreateButtonClassic>
      </div>
      <Title>Channels</Title>
      <hr />
      {channels.loading && <Spinner />}
      {channels.error && <ErrorCard error={channels.error} />}
      {channels.items && channels.items.length === 0 && (
        <EmptyList>There are no Channels.</EmptyList>
      )}
      <div className="mt-3">
        {channels.items &&
          channels.items.map((u) => <ChannelRow key={u.id} channel={u} />)}
      </div>
      <Paginator {...channels.pagination} />
    </div>
  );
};
