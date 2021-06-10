import React from "react";
import { useParams } from "react-router-dom";
import { useIntegratedSystem } from "../../../api-hooks/integratedSystemsApi";
import { useAccessToken } from "../../../api-hooks/token";
import { createWebhookReceiver } from "../../../api/integratedSystemsApi";
import { ToggleSwitch } from "../../molecules/ToggleSwitch";
import { ErrorCard } from "../../molecules/ErrorCard";
import { Spinner } from "../../molecules/Spinner";
import { Top } from "./Top";

export const CreateWebhookReceiver = () => {
  let { id } = useParams();
  const integratedSystem = useIntegratedSystem({ id });
  const token = useAccessToken();
  const [useSharedSecret, setUseSharedSecret] = React.useState(true);
  const [result, setResult] = React.useState({});

  const onClickCreate = () => {
    setResult({ loading: true });
    createWebhookReceiver({
      success: setResult,
      error: (e) => setResult({ error: e }),
      token,
      id,
      useSharedSecret,
    });
  };

  if (integratedSystem.loading) {
    return (
      <React.Fragment>
        <Top integratedSystem={integratedSystem} />
        <hr />
        <Spinner>Loading System Information</Spinner>
      </React.Fragment>
    );
  }

  return (
    <React.Fragment>
      <Top integratedSystem={integratedSystem} />
      <hr />
      Use Shared Secret
      <ToggleSwitch
        id="id"
        checked={useSharedSecret}
        onChange={setUseSharedSecret}
      />
      <button className="btn btn-primary" onClick={onClickCreate}>
        Create Webhook Receiver
      </button>
      <div className="mt-3">
        {result.loading && <Spinner>Creating Webhook</Spinner>}
        {result && result.error && <ErrorCard error={result.error} />}
        {result.endpointId && (
          <div className="card">
            <div className="card-body">
              <div>Endpoint Id: {result.endpointId}</div>
              <div>Shared Secret: {result.sharedSecret}</div>
            </div>
          </div>
        )}
      </div>
    </React.Fragment>
  );
};
