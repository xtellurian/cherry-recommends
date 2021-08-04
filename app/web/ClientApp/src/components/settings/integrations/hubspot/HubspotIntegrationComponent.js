import React from "react";
import { useParams } from "react-router-dom";
import { HubspotContactProperties } from "./HubspotContactProperties";
// import { HubspotContactEvents } from "./HubspotContactEvents";
import { HubspotWebhookBehaviour } from "./WebhookBehaviour";
import { TabActivator, Tabs } from "../../../molecules/Tabs";
import { useIntegratedSystem } from "../../../../api-hooks/integratedSystemsApi";
import { useHubspotAccount } from "../../../../api-hooks/hubspotApi";
import { BackButton, EmptyList, ErrorCard, Spinner } from "../../../molecules";
import { CrmCardBehaviour } from "./CrmCardBehaviour";

const tabs = [
  {
    id: "webhook-behaviour",
    label: "Webhook Behaviour",
  },
  {
    id: "crm-card",
    label: "CRM Card",
  },
  {
    id: "properties",
    label: "Contact Properties",
  },
  // {
  //   id: "events",
  //   label: "Events",
  // },
];
export const HubspotIntegrationComponent = () => {
  const { id } = useParams();
  const integratedSystem = useIntegratedSystem({ id });
  const hubspotAccount = useHubspotAccount({ id: integratedSystem.id });

  if (integratedSystem.loading || hubspotAccount.loading) {
    return <Spinner />;
  }
  if (hubspotAccount.error) {
    return <ErrorCard error={hubspotAccount.error} />;
  }

  if (!hubspotAccount.portalId) {
    return (
      <React.Fragment>
        <EmptyList>
          <p>Hubspot Account must be connected.</p>
          <BackButton className="" to={`/settings/integrations/detail/${id}`}>
            Overview
          </BackButton>
        </EmptyList>
      </React.Fragment>
    );
  }

  return (
    <React.Fragment>
      <Tabs tabs={tabs} defaultTabId={tabs[0].id} />
      <hr />
      <TabActivator tabId={tabs[0].id} defaultTabId={tabs[0].id}>
        <HubspotWebhookBehaviour integratedSystem={integratedSystem} />
      </TabActivator>
      <TabActivator tabId={tabs[1].id} defaultTabId={tabs[0].id}>
        <CrmCardBehaviour integratedSystem={integratedSystem} />
      </TabActivator>
      <TabActivator tabId={tabs[2].id} defaultTabId={tabs[0].id}>
        <HubspotContactProperties integratedSystem={integratedSystem} />
      </TabActivator>
      {/* <TabActivator tabId={tabs[2].id} defaultTabId={tabs[0].id}>
        <HubspotContactEvents integratedSystem={integratedSystem} />
      </TabActivator> */}
    </React.Fragment>
  );
};
