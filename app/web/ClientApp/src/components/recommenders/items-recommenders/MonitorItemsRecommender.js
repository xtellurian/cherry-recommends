import React from "react";
import { useParams } from "react-router-dom";
import {
  useItemsRecommender,
  useRecommenderTrackedUserActions,
} from "../../../api-hooks/itemsRecommendersApi";
import { Tabs, TabActivator } from "../../molecules/layout/Tabs";
import { RecommendationList } from "./RecommendationList";
import { InvokationLogs } from "./InvokationLogs";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";

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

export const MonitorRecommender = () => {
  return (
    <React.Fragment>
      <ItemRecommenderLayout>
        <Tabs tabs={tabs} />
        <TabActivator tabId="recommendations" defaultTabId={defaultTabId}>
          <RecommendationList />
        </TabActivator>
        <TabActivator tabId="invokations" defaultTabId={defaultTabId}>
          <InvokationLogs />
        </TabActivator>
      </ItemRecommenderLayout>
    </React.Fragment>
  );
};
