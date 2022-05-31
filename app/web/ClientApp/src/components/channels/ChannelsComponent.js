import React from "react";
import { Route, Routes } from "react-router-dom";

import { ChannelsSummary } from "./ChannelsSummary";
import { CreateChannel } from "./CreateChannel";
import { ChannelDetail } from "./ChannelDetail";

export const ChannelsComponent = (props) => {
  return (
    <Routes>
      <Route path="channels">
        <Route index element={<ChannelsSummary />} />
        <Route path="create" element={<CreateChannel />} />
        <Route path="detail/:id" element={<ChannelDetail />} />
      </Route>
    </Routes>
  );
};
