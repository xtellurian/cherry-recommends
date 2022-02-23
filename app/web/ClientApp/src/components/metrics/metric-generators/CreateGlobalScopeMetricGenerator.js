import React from "react";
import { Subtitle, Title } from "../../molecules";
import {
  TabActivator,
  StatefulTabs,
} from "../../molecules/layout/StatefulTabs";

import { CreateAggregateCustomerGenerator } from "./global/CreateAggregateCustomerGenerator";
import { CreateJoinMetricsGenerator } from "./global/CreateJoinMetricsGenerator";

const tabs = [
  {
    id: "aggregate",
    label: "Customer Aggregate",
  },
  {
    id: "join",
    label: "Ratio of Metrics",
  },
];
export const CreateGlobalScopeMetricGenerator = ({ metric, onCreated }) => {
  const [currentTabId, setCurrentTabId] = React.useState(tabs[0].id);

  return (
    <>
      <Title>Metric Generation</Title>
      <Subtitle>{metric.name}</Subtitle>
      <StatefulTabs
        currentTabId={currentTabId}
        setCurrentTabId={setCurrentTabId}
        tabs={tabs}
      />
      <TabActivator tabId="aggregate" currentTabId={currentTabId}>
        <CreateAggregateCustomerGenerator
          metric={metric}
          onCreated={onCreated}
        />
      </TabActivator>
      <TabActivator tabId="join" currentTabId={currentTabId}>
        <CreateJoinMetricsGenerator
          metric={metric}
          onCreated={onCreated}
        />
      </TabActivator>
    </>
  );
};
