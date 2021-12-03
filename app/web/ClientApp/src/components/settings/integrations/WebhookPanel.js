import React from "react";
import { Link } from "react-router-dom";
import { useWebhookReceivers } from "../../../api-hooks/integratedSystemsApi";
import { Subtitle } from "../../molecules/layout";
import { EmptyList } from "../../molecules/";
import { Spinner } from "../../molecules/Spinner";
import { ErrorCard } from "../../molecules/ErrorCard";
import { ExpandableCard } from "../../molecules/ExpandableCard";
import { CopyableField } from "../../molecules/fields/CopyableField";

const basePath = `${window.location.protocol}//${window.location.host}`;
export const WebhookReceiverRow = ({ wr }) => {
  return (
    <ExpandableCard label={wr.endpointId}>
      <CopyableField label="Endpoint ID" value={wr.endpointId} />
      <CopyableField
        label="Endpoint"
        value={`${basePath}/api/webhooks/receivers/${wr.endpointId}`}
      />
      <CopyableField
        label="Shared Secret"
        value={wr.sharedSecret}
        isSecret={true}
      />
    </ExpandableCard>
  );
};

const Top = ({ integratedSystemId }) => {
  return (
    <React.Fragment>
      <Link
        to={`/settings/integrations/create-webhook-receiver/${integratedSystemId}`}
      >
        <button className="btn btn-primary float-right">Create Webhook</button>
      </Link>
      <Subtitle>Webhooks</Subtitle>
    </React.Fragment>
  );
};
export const WebhookPanel = ({ integratedSystemId, className }) => {
  const webhookReceivers = useWebhookReceivers({ id: integratedSystemId });

  return (
    <div className={`${className || "mt-2"}`}>
      <div className="m-3">
        <Top integratedSystemId={integratedSystemId} />
      </div>
      {webhookReceivers.loading && <Spinner>Loading Webhook Receivers</Spinner>}
      {webhookReceivers.items && webhookReceivers.items.length === 0 && (
        <EmptyList>No Webhook Receivers</EmptyList>
      )}

     
        {webhookReceivers.error && <ErrorCard error={webhookReceivers.error} />}
        {!webhookReceivers.loading &&
          webhookReceivers.map((wr) => (
            <WebhookReceiverRow key={wr.id} wr={wr} />
          ))}
 
    </div>
  );
};
