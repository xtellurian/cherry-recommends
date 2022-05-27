import React from "react";

import { Tabs } from "../../molecules/layout/Tabs";

export const tabs = [
  {
    id: "detail",
    label: "Details",
  },
  {
    id: "test",
    label: "Test",
  },
  {
    id: "reports",
    label: "Reports",
  },
  {
    id: "delivery",
    label: "Delivery",
  },
  {
    id: "settings",
    label: "Settings",
  },
];

const baseURL = "/campaigns/promotions-campaigns";

export const PromotionsCampaignPrimaryNav = ({ id }) => {
  const newTabs = tabs.map((tab) => ({
    ...tab,
    pathname: `${baseURL}/${tab.id}/${id}`,
  }));

  return (
    <div className="my-4">
      <Tabs tabs={newTabs} defaultTabId={tabs[0].id} />
    </div>
  );
};
