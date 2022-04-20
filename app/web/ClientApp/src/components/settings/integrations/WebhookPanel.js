import React from "react";
import {
  useIntegratedSystem,
  useWebhookReceivers,
} from "../../../api-hooks/integratedSystemsApi";
import { EmptyList } from "../../molecules/";
import { Navigation } from "../../molecules/Navigation";
import { Spinner } from "../../molecules/Spinner";
import { ErrorCard } from "../../molecules/ErrorCard";
import { ExpandableCard } from "../../molecules/ExpandableCard";
import { CopyableField } from "../../molecules/fields/CopyableField";
import { useTenantName } from "../../tenants/PathTenantProvider";
import { CardSection, Label } from "../../molecules/layout/CardSection";
import { SmallPopup } from "../../molecules/popups/SmallPopup";
import { CreateWebhookReceiver } from "./CreateWebhookReceiver";

const basePath = `${window.location.protocol}//${window.location.host}`;
export const WebhookReceiverRow = ({ wr }) => {
  const { tenantName } = useTenantName();
  const tenantParam = tenantName !== "" ? `?x-tenant=${tenantName}` : "";
  return (
    <ExpandableCard label={wr.endpointId}>
      <CopyableField
        label="Endpoint"
        value={`${basePath}/api/webhooks/receivers/${wr.endpointId}${tenantParam}`}
      />
      <CopyableField
        label="Shared Secret"
        value={wr.sharedSecret}
        isSecret={true}
      />
    </ExpandableCard>
  );
};

const AddWebhookPopup = ({
  integratedSystem,
  webhookPopupOpen,
  setWebhookPopupOpen,
  onCreated,
}) => {
  return (
    <SmallPopup isOpen={webhookPopupOpen} setIsOpen={setWebhookPopupOpen}>
      <div className="p-2">
        <CreateWebhookReceiver
          integratedSystem={integratedSystem}
          onCreated={onCreated}
        />
      </div>
    </SmallPopup>
  );
};

const Top = ({ integratedSystem, onCreated }) => {
  const [webhookPopupOpen, setWebhookPopupOpen] = React.useState(false);
  return (
    <React.Fragment>
      <AddWebhookPopup
        integratedSystem={integratedSystem}
        webhookPopupOpen={webhookPopupOpen}
        setWebhookPopupOpen={setWebhookPopupOpen}
        onCreated={(x) => {
          setWebhookPopupOpen(false);
          onCreated(x);
        }}
      />
      <div className="d-flex justify-content-between">
        <Label>Webhooks</Label>

        <button
          className="btn btn-primary"
          onClick={() => setWebhookPopupOpen(true)}
        >
          Add Webhook Endpoint
        </button>
      </div>
    </React.Fragment>
  );
};
export const WebhookPanel = ({ integratedSystemId, className }) => {
  const [trigger, setTrigger] = React.useState({});
  const webhookReceivers = useWebhookReceivers({
    id: integratedSystemId,
    trigger,
  });
  const integratedSystem = useIntegratedSystem({ id: integratedSystemId });

  return (
    <CardSection>
      <div className={`${className || "mt-2"}`}>
        <div className="m-3">
          <Top
            integratedSystem={integratedSystem}
            onCreated={(x) => {
              setTrigger(x);
            }}
          />
        </div>
        {webhookReceivers.loading && (
          <Spinner>Loading Webhook Receivers</Spinner>
        )}
        {webhookReceivers.items && webhookReceivers.items.length === 0 && (
          <EmptyList>No Webhook Receivers</EmptyList>
        )}

        {webhookReceivers.error && <ErrorCard error={webhookReceivers.error} />}
        {!webhookReceivers.loading &&
          !webhookReceivers.error &&
          webhookReceivers.map((wr) => (
            <WebhookReceiverRow key={wr.id} wr={wr} />
          ))}
      </div>
    </CardSection>
  );
};
