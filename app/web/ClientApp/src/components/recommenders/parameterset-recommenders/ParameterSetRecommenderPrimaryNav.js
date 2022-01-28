import React from "react";
import {
  PrimaryNavigationMenu,
  NavListItem,
} from "../../molecules/layout/PrimaryNavigationMenu";

export const ParameterSetRecommenderPrimaryNav = ({ id }) => {
  return (
    <PrimaryNavigationMenu>
      {/* <NavListItem to={`/recommenders/parameter-set-recommenders/overview/${id}`}>
        Overview
      </NavListItem> */}
      <NavListItem to={`/recommenders/parameter-set-recommenders/detail/${id}`}>
        Detail
      </NavListItem>
      <NavListItem
        to={`/recommenders/parameter-set-recommenders/monitor/${id}`}
      >
        Monitor
      </NavListItem>
      <NavListItem to={`/recommenders/parameter-set-recommenders/test/${id}`}>
        Invoke
      </NavListItem>
      <NavListItem
        to={`/recommenders/parameter-set-recommenders/triggers/${id}`}
      >
        Triggers
      </NavListItem>
      <NavListItem
        to={`/recommenders/parameter-set-recommenders/destinations/${id}`}
      >
        Destinations
      </NavListItem>
      <NavListItem
        to={`/recommenders/parameter-set-recommenders/learning-metrics/${id}`}
      >
        Learning Metrics
      </NavListItem>

      <NavListItem
        to={`/recommenders/parameter-set-recommenders/arguments/${id}`}
      >
        Arguments
      </NavListItem>
      <NavListItem
        to={`/recommenders/parameter-set-recommenders/settings/${id}`}
      >
        Settings
      </NavListItem>
    </PrimaryNavigationMenu>
  );
};
