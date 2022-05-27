import React from "react";
import { Tabs } from "../../../molecules/layout/Tabs";

const tabs = [
  {
    id: "promotions",
    label: "Promotions",
  },
  {
    id: "weights",
    label: "Weights",
  },
];

const baseURL = "/campaigns/promotions-campaigns/manage";

export const ManageNav = ({ id }) => {
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
