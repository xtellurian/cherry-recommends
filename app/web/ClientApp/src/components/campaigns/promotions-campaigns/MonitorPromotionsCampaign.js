import React from "react";

import { ItemsRecommendationList } from "./RecommendationList";
import { InvokationLogs } from "./InvokationLogs";
import {
  StatefulTabs,
  TabActivator,
} from "../../molecules/layout/StatefulTabs";
import { OfferList } from "./OfferList";

export const MonitorCampaign = ({ trigger }) => {
  const [currentTabId, setCurrentTabId] = React.useState();

  const tabs = [
    {
      id: "recommendations",
      label: "Latest Recommendations",
      render: () => <ItemsRecommendationList trigger={trigger} size="lg" />,
    },
    {
      id: "invokations",
      label: "Invokations",
      render: () => <InvokationLogs trigger={trigger} />,
    },
  ];

  React.useEffect(() => {
    setCurrentTabId(tabs[0].id);
  }, []);

  return (
    <React.Fragment>
      <StatefulTabs
        tabs={tabs}
        currentTabId={currentTabId}
        setCurrentTabId={setCurrentTabId}
      />

      {tabs.map((tab) => (
        <TabActivator key={tab.id} currentTabId={currentTabId} tabId={tab.id}>
          {tab?.render()}
        </TabActivator>
      ))}
    </React.Fragment>
  );
};
