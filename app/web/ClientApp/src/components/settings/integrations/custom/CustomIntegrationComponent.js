import React from "react";
import { useParams } from "react-router-dom";
import { TabActivator, Tabs } from "../../../molecules/layout/Tabs";
import { useIntegratedSystem } from "../../../../api-hooks/integratedSystemsApi";
import {  Spinner } from "../../../molecules";
import {CustomIntegrationSummary} from "./CustomIntegrationSummary"

const tabs = [
  {
    id: "summary",
    label: "Summary",
  },
];
export const CustomIntegrationComponent = () => {
  const { id } = useParams();
  const integratedSystem = useIntegratedSystem({ id });

  if (integratedSystem.loading) {
    return <Spinner />;
  }

  return (
    <React.Fragment>
      <Tabs tabs={tabs} defaultTabId={tabs[0].id} />
      <TabActivator tabId={tabs[0].id} defaultTabId={tabs[0].id}>
        <CustomIntegrationSummary integratedSystem={integratedSystem} />
      </TabActivator>
      
    </React.Fragment>
  );
};
