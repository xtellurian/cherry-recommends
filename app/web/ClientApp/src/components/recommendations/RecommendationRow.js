import React from "react";
import { CaretRightFill } from "react-bootstrap-icons";
import { useCustomer } from "../../api-hooks/customersApi";
import { Title, Subtitle } from "../molecules";
import { DateTimeField } from "../molecules/DateTimeField";
import { EntityField } from "../molecules/EntityField";
import { JsonView } from "../molecules/JsonView";
import FlexRow from "../molecules/layout/EntityFlexRow";
import { StatefulTabs, TabActivator } from "../molecules/layout/StatefulTabs";
import { BigPopup } from "../molecules/popups/BigPopup";

const tabs = [
  { id: "output", label: "Output" },
  { id: "customer", label: "Customer" },
  { id: "input", label: "Input" },
];
const RecommendationDetail = ({ recommendation }) => {
  const [currentTabId, setCurrentTabId] = React.useState(tabs[0].id);
  const customer = useCustomer({
    id: recommendation.trackedUserId,
    useInternalId: true,
  });
  const modelInput = JSON.parse(recommendation.modelInput);
  const modelOutput = JSON.parse(recommendation.modelOutput);
  return (
    <React.Fragment>
      <Title>Recommendation</Title>
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
      <TabActivator tabId="customer" currentTabId={currentTabId}>
        <EntityField
          entity={customer}
          label="Customer"
          to={`customers/detail/${customer.id}`}
        />
      </TabActivator>
      <TabActivator tabId="input" currentTabId={currentTabId}>
        <JsonView data={modelInput} />
      </TabActivator>
    </React.Fragment>
  );
};
export const RecommendationRow = ({ recommendation }) => {
  const [isPopupOpen, setIsPopupOpen] = React.useState(false);

  return (
    <React.Fragment>
      <BigPopup isOpen={isPopupOpen} setIsOpen={setIsPopupOpen}>
        <RecommendationDetail recommendation={recommendation} />
      </BigPopup>
      <FlexRow
        className="clickable-row"
        onClick={() => setIsPopupOpen(true)}
        style={{ cursor: "pointer" }}
      >
        <div>
          <CaretRightFill style={{ color: "var(--cherry-pink)" }} />
          <span className="ml-2">{recommendation.recommenderType}</span>
        </div>
        <DateTimeField date={recommendation.created} />
      </FlexRow>
    </React.Fragment>
  );
};
