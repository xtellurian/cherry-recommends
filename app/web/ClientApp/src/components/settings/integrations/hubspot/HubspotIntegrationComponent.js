import React from "react";
import { useParams } from "react-router-dom";
import { HubspotContactProperties } from "./HubspotContactProperties";
import { HubspotContactEvents } from "./HubspotContactEvents";
import { TabActivator, Tabs } from "../../../molecules/Tabs";
import { useIntegratedSystem } from "../../../../api-hooks/integratedSystemsApi";

const tabs = [
  {
    id: "properties",
    label: "Contact Properties",
  },
  {
    id: "Events",
    label: "Events",
  },
];
export const HubspotIntegrationComponent = () => {
  const { id } = useParams();
  const integratedSystem = useIntegratedSystem({ id });
  return (
    <React.Fragment>
      <Tabs tabs={tabs} defaultTabId={tabs[0].id} />
      <hr />
      <TabActivator tabId={tabs[0].id} defaultTabId={tabs[0].id}>
        <HubspotContactProperties integratedSystem={integratedSystem} />
      </TabActivator>
      <TabActivator tabId={tabs[1].id} defaultTabId={tabs[0].id}>
        <HubspotContactEvents integratedSystem={integratedSystem} />
      </TabActivator>
    </React.Fragment>
  );
};
