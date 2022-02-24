import React from "react";
import {
  PrimaryNavigationMenu,
  NavListItem,
} from "../../molecules/layout/PrimaryNavigationMenu";

export const ItemsRecommenderPrimaryNav = ({ id }) => {
  return (
    <PrimaryNavigationMenu>
      {/* <NavListItem to={`/recommenders/promotions-recommenders/overview/${id}`}>
        Overview
      </NavListItem> */}
      <NavListItem to={`/recommenders/promotions-recommenders/detail/${id}`}>
        Details
      </NavListItem>
      <NavListItem to={`/recommenders/promotions-recommenders/monitor/${id}`}>
        Monitor
      </NavListItem>
      <NavListItem
        to={`/recommenders/promotions-recommenders/performance/${id}`}
      >
        Performance
      </NavListItem>
      <NavListItem to={`/recommenders/promotions-recommenders/test/${id}`}>
        Test
      </NavListItem>
      <NavListItem to={`/recommenders/promotions-recommenders/triggers/${id}`}>
        Triggers
      </NavListItem>
      <NavListItem
        to={`/recommenders/promotions-recommenders/destinations/${id}`}
      >
        Destinations
      </NavListItem>
      <NavListItem
        to={`/recommenders/promotions-recommenders/learning-metrics/${id}`}
      >
        Learning Metrics
      </NavListItem>
      <NavListItem to={`/recommenders/promotions-recommenders/arguments/${id}`}>
        Arguments
      </NavListItem>
      <NavListItem to={`/recommenders/promotions-recommenders/settings/${id}`}>
        Settings
      </NavListItem>
    </PrimaryNavigationMenu>
  );
};
