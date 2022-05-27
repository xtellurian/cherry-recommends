import React from "react";
import {
  PrimaryNavigationMenu,
  NavListItem,
} from "../../molecules/layout/PrimaryNavigationMenu";

export const ParameterSetCampaignPrimaryNav = ({ id }) => {
  return (
    <PrimaryNavigationMenu>
      {/* <NavListItem to={`/recommenders/parameter-set-recommenders/overview/${id}`}>
        Overview
      </NavListItem> */}
      <NavListItem to={`/campaigns/parameter-set-campaigns/detail/${id}`}>
        Detail
      </NavListItem>
      <NavListItem to={`/campaigns/parameter-set-campaigns/monitor/${id}`}>
        Monitor
      </NavListItem>
      <NavListItem to={`/campaigns/parameter-set-campaigns/test/${id}`}>
        Invoke
      </NavListItem>
      <NavListItem to={`/campaigns/parameter-set-campaigns/triggers/${id}`}>
        Triggers
      </NavListItem>
      <NavListItem to={`/campaigns/parameter-set-campaigns/destinations/${id}`}>
        Destinations
      </NavListItem>
      <NavListItem
        to={`/campaigns/parameter-set-campaigns/learning-metrics/${id}`}
      >
        Learning Metrics
      </NavListItem>

      <NavListItem to={`/campaigns/parameter-set-campaigns/arguments/${id}`}>
        Arguments
      </NavListItem>
      <NavListItem to={`/campaigns/parameter-set-campaigns/settings/${id}`}>
        Settings
      </NavListItem>
    </PrimaryNavigationMenu>
  );
};
