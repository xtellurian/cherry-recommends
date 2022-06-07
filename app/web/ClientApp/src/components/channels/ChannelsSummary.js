import React from "react";
import { useChannels } from "../../api-hooks/channelsApi";
import { Spinner, Paginator } from "../molecules";
import { EmptyList } from "../molecules";
import { ChannelRow } from "./ChannelRow";
import Layout, {
  CreateEntityButton,
} from "../molecules/layout/EntitySummaryLayout";

export const ChannelsSummary = () => {
  const channels = useChannels();

  return (
    <Layout
      header="Channels"
      createButton={
        <CreateEntityButton to="/integrations/channels/create">
          Create Channel
        </CreateEntityButton>
      }
      error={channels.error}
    >
      {channels.loading && <Spinner />}
      {channels.items && channels.items.length === 0 && (
        <EmptyList>There are no Channels.</EmptyList>
      )}
      <div className="mt-3">
        {channels.items &&
          channels.items.map((u) => <ChannelRow key={u.id} channel={u} />)}
      </div>
      <Paginator {...channels.pagination} />
    </Layout>
  );
};
