import React from "react";
import { useParams } from "react-router-dom";
import { useIntegratedSystem } from "../../../api-hooks/integratedSystemsApi";
import { useAccessToken } from "../../../api-hooks/token";
import { createWebhookReceiverAsync } from "../../../api/integratedSystemsApi";
import { ToggleSwitch } from "../../molecules/ToggleSwitch";
import { ErrorCard } from "../../molecules/ErrorCard";
import { Spinner } from "../../molecules/Spinner";
import { Top } from "./Top";
import { WebhookReceiverRow } from "./WebhookPanel";
import { Subtitle } from "../../molecules";

export const CreateWebhookReceiverPage = () => {
  let { id } = useParams();
  const integratedSystem = useIntegratedSystem({ id });
  return (
    <React.Fragment>
      <Top integratedSystem={integratedSystem} />
      <hr />
      <CreateWebhookReceiver integratedSystem={integratedSystem} />;
    </React.Fragment>
  );
};

export const CreateWebhookReceiver = ({ integratedSystem, onCreated }) => {
  const token = useAccessToken();
  const [useSharedSecret, setUseSharedSecret] = React.useState(true);
  const [result, setResult] = React.useState({});

  const onClickCreate = () => {
    setResult({ loading: true });
    createWebhookReceiverAsync({
      token,
      id: integratedSystem.id,
      useSharedSecret,
    })
      .then(setResult)
      .then(() => {
        if (typeof onCreated === "function") {
          onCreated({});
        }
      })
      .catch((error) => setResult({ error }));
  };

  if (integratedSystem.loading) {
    return (
      <React.Fragment>
        <Spinner>Loading System Information</Spinner>
      </React.Fragment>
    );
  }

  return (
    <React.Fragment>
      <Subtitle>Add Webhook Endpoint</Subtitle>
      <div className="d-flex justify-content-center mb-3">
        <div className="m-2">Use Shared Secret</div>
        <ToggleSwitch
          id="id"
          checked={useSharedSecret}
          onChange={setUseSharedSecret}
        />
      </div>
      <button className="btn btn-primary" onClick={onClickCreate}>
        Add Webhook Endpoint
      </button>
      <div className="mt-3">
        {result.loading && <Spinner>Creating Webhook</Spinner>}
        {result && result.error && <ErrorCard error={result.error} />}

        {result && result.endpointId && <WebhookReceiverRow wr={result} />}
      </div>
    </React.Fragment>
  );
};
