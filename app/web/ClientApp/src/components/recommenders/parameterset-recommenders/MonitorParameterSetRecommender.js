import React from "react";
import { useParams } from "react-router-dom";
import {
  useParameterSetRecommender,
  useRecommenderTrackedUserActions,
} from "../../../api-hooks/parameterSetRecommendersApi";
import { Subtitle, Title, BackButton, Spinner } from "../../molecules";
import { Tabs, TabActivator } from "../../molecules/layout/Tabs";
import { RevenueChartUtil } from "../utils/revenueChartUtil";
import { RecommendationList } from "./RecommendationList";
import { TargetVariableValues } from "./TargetVariableValues";
import { InvokationLogs } from "./InvokationLogs";
import { ParameterSetRecommenderLayout } from "./ParameterSetRecommenderLayout";

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

export const MonitorParameterSetRecommender = () => {

  return (
    <React.Fragment>
      <ParameterSetRecommenderLayout>
        <Tabs tabs={tabs} />

        <TabActivator tabId="recommendations" defaultTabId={defaultTabId}>
          <RecommendationList />
        </TabActivator>
        <TabActivator tabId="invokations" defaultTabId={defaultTabId}>
          <InvokationLogs />
        </TabActivator>
      </ParameterSetRecommenderLayout>
    </React.Fragment>
  );
};
