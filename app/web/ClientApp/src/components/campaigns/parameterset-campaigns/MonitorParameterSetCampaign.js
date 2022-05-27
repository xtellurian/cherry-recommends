import React from "react";
import { Tabs, TabActivator } from "../../molecules/layout/Tabs";
import { ParameterRecommendationList } from "./RecommendationList";
import { InvokationLogs } from "./InvokationLogs";
import { ParameterSetCampaignLayout } from "./ParameterSetCampaignLayout";

const tabs = [
  {
    id: "recommendations",
    label: "Latest Recommendations",
  },
  {
    id: "invokations",
    label: "Invokations",
  },
];

const defaultTabId = tabs[0].id;

export const MonitorParameterSetCampaign = () => {
  return (
    <React.Fragment>
      <ParameterSetCampaignLayout>
        <Tabs tabs={tabs} />

        <TabActivator tabId="recommendations" defaultTabId={defaultTabId}>
          <ParameterRecommendationList />
        </TabActivator>
        <TabActivator tabId="invokations" defaultTabId={defaultTabId}>
          <InvokationLogs />
        </TabActivator>
      </ParameterSetCampaignLayout>
    </React.Fragment>
  );
};
