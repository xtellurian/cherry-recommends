import React from "react";
import { useParams } from "react-router-dom";
import { Check } from "react-bootstrap-icons";
import { Subtitle } from "../../molecules/PageHeadings";
import { useHubspotAccount } from "../../../api-hooks/hubspotApi";
import { Top } from "./Top";
import { WebookPanel } from "./WebhookPanel";
import { useIntegratedSystem } from "../../../api-hooks/integratedSystemsApi";
import { Spinner } from "../../molecules/Spinner";
import { ErrorCard } from "../../molecules/ErrorCard";
import { HubspotDetail } from "./hubspot/HubspotDetail";

export const IntegratedSystemDetail = () => {
  let { id } = useParams();
  const integratedSystem = useIntegratedSystem({ id });
  

  return (
    <React.Fragment>
      <Top integratedSystem={integratedSystem} />
      <hr />
      {integratedSystem.loading && (
        <Spinner>Loading System Information</Spinner>
      )}
      {integratedSystem.integrationStatus === "ok" && (
        <div className="text-success float-right">
          <Check size={42} className="mr-1" />
          Connected
        </div>
      )}
      {!integratedSystem.loading && !integratedSystem.error && (
        <div className="col-4">
          <Subtitle>Information</Subtitle>
          <div>Name: {integratedSystem.name}</div>
          <div>Type: {integratedSystem.systemType}</div>
          <div>Status: {integratedSystem.integrationStatus}</div>
        </div>
      )}

        <hr/>
      {!integratedSystem.loading && !integratedSystem.error && (
        <div className="row">
          {integratedSystem &&
            integratedSystem.id &&
            integratedSystem.systemType === "segment" && (
              <div className="col">
                <WebookPanel integratedSystemId={integratedSystem.id} />
              </div>
            )}
        </div>
      )}

      <div className="row mt-3">
        <div className="col">
          {integratedSystem.error && (
            <ErrorCard error={integratedSystem.error} />
          )}
          {integratedSystem.systemType === "hubspot" && (
            <HubspotDetail integratedSystem={integratedSystem} />
          )}
        </div>
      </div>
    </React.Fragment>
  );
};
