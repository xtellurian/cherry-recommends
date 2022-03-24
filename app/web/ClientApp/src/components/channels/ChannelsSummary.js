import React from "react";
import { CreateButtonClassic } from "../molecules/CreateButton";
import { Title } from "../molecules";

export const ChannelsSummary = () => {
  return (
    <div>
      <div className="float-right">
        <CreateButtonClassic to="/channels/create">
          Create New
        </CreateButtonClassic>
      </div>
      <Title>Channels</Title>
      <hr />
    </div>
  );
};
