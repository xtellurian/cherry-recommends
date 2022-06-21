import React from "react";
import { useAccessToken } from "../../../../api-hooks/token";
import { useQuery } from "../../../../utility/utility";
import { useIntegratedSystem } from "../../../../api-hooks/integratedSystemsApi";
import { useHubspotAppInformation } from "../../../../api-hooks/hubspotApi";
import { saveHubspotCodeAsync } from "../../../../api/hubspotApi";
import { Title } from "../../../molecules/layout";
import { Spinner } from "../../../molecules/Spinner";
import { ErrorCard } from "../../../molecules/ErrorCard";
import { MoveUpHierarchyButton, Navigation } from "../../../molecules";
import { useTenantName } from "../../../tenants/PathTenantProvider";

const basePath = `${window.location.protocol}//${window.location.host}`;
const redirectUri = `${basePath}/_connect/hubspot/callback`;

const stages = ["READY", "INSTALLING", "SAVING", "COMPLETE"];
const Top = () => {
  return (
    <React.Fragment>
      <MoveUpHierarchyButton
        to="/settings/integrations"
        className="float-right"
      >
        Integrations
      </MoveUpHierarchyButton>
      <Title>Hubspot Installation</Title>
      <hr />
    </React.Fragment>
  );
};

const ProgressView = ({ stage }) => {
  const finished = stage === stages[3];

  return (
    <div className={`card w-50 m-auto ${finished ? "bg-success" : ""}`}>
      <div className="card-body text-center">Installation: {stage}</div>
    </div>
  );
};
const SystemStateView = ({ integratedSystem }) => {
  if (integratedSystem.loading) {
    return <Spinner>Loading System Details</Spinner>;
  }
  if (integratedSystem.integrationStatus === "ok") {
    return (
      <div className="card w-50 m-auto">
        <div className="card-body text-center bg-success">
          Integration Status: {integratedSystem.integrationStatus}
          <div>
            <Navigation
              to={{
                pathname: `/settings/integrations/detail/${integratedSystem.id}`,
                search: null,
              }}
            >
              <button className="btn btn-primary btn-block">
                View Integration
              </button>
            </Navigation>
          </div>
        </div>
      </div>
    );
  } else {
    return (
      <div className="card w-50 m-auto">
        <div className="card-body text-center">
          Integration Status: {integratedSystem.integrationStatus}
        </div>
      </div>
    );
  }
};
export const HubspotConnector = () => {
  const query = useQuery();
  const integratedSystemId = query.get("state");
  const integratedSystem = useIntegratedSystem({ id: integratedSystemId });
  const [stage, setStage] = React.useState(stages[0]);
  const [error, setError] = React.useState();
  const { clientId, scope, loading } = useHubspotAppInformation();
  const { tenantName } = useTenantName();

  const jsonState = { id: `${integratedSystemId}`, tenant: `${tenantName}` };
  const state = JSON.stringify(jsonState);

  const installLink = `https://app.hubspot.com/oauth/authorize?client_id=${clientId}&redirect_uri=${redirectUri}&scope=${scope}&state=${state}`;

  return (
    <React.Fragment>
      <Top />
      {loading && <Spinner>Loading Hubspot App Information</Spinner>}
      {error && <ErrorCard error={error} />}
      <SystemStateView integratedSystem={integratedSystem} />
      <ProgressView stage={stage} />

      <div className="text-center m-5">
        <a href={installLink}>
          <button
            disabled={loading}
            onClick={() => setStage(stages[1])}
            className="btn btn-primary"
          >
            {loading ? "Loading App Info" : "Install Hubspot App"}
          </button>
        </a>
      </div>
    </React.Fragment>
  );
};
