import React from "react";
import { usePromotionsRecommendation } from "../../api-hooks/promotionsCampaignsApi";
import { Spinner, Subtitle, Title } from "../molecules";
import { DateTimeField } from "../molecules/DateTimeField";
import { JsonView } from "../molecules/JsonView";
import { StatefulTabs, TabActivator } from "../molecules/layout/StatefulTabs";
import { OfferSection } from "./OfferSection";

const tabs = [
  { id: "output", label: "Output" },
  { id: "offer", label: "Offer" },
  { id: "input", label: "Input" },
];
export const RecommendationDetail = ({ recommendationId }) => {
  const [currentTabId, setCurrentTabId] = React.useState(tabs[0].id);
  const recommendation = usePromotionsRecommendation({
    recommendationId: recommendationId,
  });
  const modelInput = recommendation.modelInput
    ? JSON.parse(recommendation.modelInput)
    : {};
  const modelOutput = recommendation.modelOutput
    ? JSON.parse(recommendation.modelOutput)
    : {};
  return (
    <React.Fragment>
      <Title>Recommendation</Title>
      {recommendation.loading && <Spinner />}
      <Subtitle>{recommendation.id}</Subtitle>
      <DateTimeField date={recommendation.created} />
      <StatefulTabs
        tabs={tabs}
        currentTabId={currentTabId}
        setCurrentTabId={setCurrentTabId}
      />

      <TabActivator tabId="output" currentTabId={currentTabId}>
        <JsonView
          data={modelOutput}
          shouldExpandNode={(n) => n.includes("scoredItems")}
        />
      </TabActivator>
      <TabActivator tabId="offer" currentTabId={currentTabId}>
        <OfferSection
          recommendation={recommendation}
          customer={recommendation.customer}
          business={recommendation.business}
        />
      </TabActivator>
      <TabActivator tabId="input" currentTabId={currentTabId}>
        <JsonView data={modelInput} />
      </TabActivator>
    </React.Fragment>
  );
};
