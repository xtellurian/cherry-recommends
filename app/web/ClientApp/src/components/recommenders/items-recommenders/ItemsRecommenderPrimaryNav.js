import React from "react";
import {
  PrimaryNavigationMenu,
  NavListItem,
} from "../../molecules/layout/PrimaryNavigationMenu";

export const ItemsRecommenderPrimaryNav = ({ id }) => {
  return (
    <PrimaryNavigationMenu>
      <NavListItem to={`/recommenders/items-recommenders/detail/${id}`}>
        Details
      </NavListItem>
      <NavListItem to={`/recommenders/items-recommenders/monitor/${id}`}>
        Monitor
      </NavListItem>
      <NavListItem to={`/recommenders/items-recommenders/test/${id}`}>
        Test
      </NavListItem>
      <NavListItem to={`/recommenders/items-recommenders/triggers/${id}`}>
        Triggers
      </NavListItem>
      <NavListItem to={`/recommenders/items-recommenders/destinations/${id}`}>
        Destinations
      </NavListItem>
      <NavListItem
        to={`/recommenders/items-recommenders/learning-features/${id}`}
      >
        Learning Features
      </NavListItem>
      <NavListItem to={`/recommenders/items-recommenders/arguments/${id}`}>
        Arguments
      </NavListItem>
      <NavListItem to={`/recommenders/items-recommenders/settings/${id}`}>
        Settings
      </NavListItem>
    </PrimaryNavigationMenu>
  );
};
