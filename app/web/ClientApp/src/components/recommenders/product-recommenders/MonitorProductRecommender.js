import React from "react";
import { useParams } from "react-router-dom";
import {
  useProductRecommender,
  useRecommenderTrackedUserActions,
} from "../../../api-hooks/productRecommendersApi";
import { Subtitle, Title, BackButton, Spinner } from "../../molecules";
import { Tabs, TabActivator } from "../../molecules/Tabs";
import { RevenueChartUtil } from "../utils/revenueChartUtil";
import { RecommendationList } from "./RecommendationList";
import { TargetVariableValues } from "./TargetVariableValues";
import { InvokationLogs } from "./InvokationLogs";

const tabs = [
  {
    id: "recommendations",
    label: "Latest Recommendations",
  },
  {
    id: "revenue",
    label: "Revenue",
  },
  {
    id: "target-variable",
    label: "Target Variable",
  },
  {
    id: "invokations",
    label: "Invokations",
  },
];

const defaultTabId = tabs[0].id;

export const MonitorProductRecommender = () => {
  const { id } = useParams();
  const recommender = useProductRecommender({ id });
  const revenueActions = useRecommenderTrackedUserActions({
    id,
    page: 1,
    revenueOnly: true,
  });

  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to={`/recommenders/product-recommenders/detail/${id}`}
      >
        Recommender
      </BackButton>
      <Title>Recommender Monitoring</Title>
      <Subtitle>{recommender.name}</Subtitle>
      <hr />

      <Tabs tabs={tabs} />

      <TabActivator tabId="revenue" defaultTabId={defaultTabId}>
        {revenueActions.items && (
          <RevenueChartUtil
            recommender={recommender}
            actions={revenueActions.items}
          />
        )}
        {revenueActions.loading && <Spinner />}
      </TabActivator>

      <TabActivator tabId="recommendations" defaultTabId={defaultTabId}>
        <RecommendationList />
      </TabActivator>
      <TabActivator tabId="target-variable" defaultTabId={defaultTabId}>
        <TargetVariableValues />
      </TabActivator>
      <TabActivator tabId="invokations" defaultTabId={defaultTabId}>
        <InvokationLogs />
      </TabActivator>
    </React.Fragment>
  );
};
