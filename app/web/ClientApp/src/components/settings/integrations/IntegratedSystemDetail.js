import React from "react";
import { useParams } from "react-router-dom";
import { Subtitle } from "../../molecules/PageHeadings";
import { Top } from "./Top";
import { WebookPanel } from "./WebhookPanel";
import { useIntegratedSystem } from "../../../api-hooks/integratedSystemsApi";
import { Spinner } from "../../molecules/Spinner";
import { ErrorCard } from "../../molecules/ErrorCard";

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
      {!integratedSystem.loading && !integratedSystem.error && (
        <div className="row">
          <div className="col-4">
            <Subtitle>Information</Subtitle>
            <div>Name: {integratedSystem.name}</div>
            <div>Type: {integratedSystem.systemType}</div>
          </div>

          <div className="col">
            {integratedSystem && integratedSystem.id && (
              <WebookPanel integratedSystemId={integratedSystem.id} />
            )}
          </div>
        </div>
      )}
      {integratedSystem.error && <ErrorCard error={integratedSystem.error} />}
    </React.Fragment>
  );
};
