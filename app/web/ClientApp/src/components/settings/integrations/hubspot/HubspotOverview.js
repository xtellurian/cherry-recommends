import React from "react";
import { useHubspotAccount } from "../../../../api-hooks/hubspotApi";
import { Navigation } from "../../../molecules";

export const HubspotOverview = ({ integratedSystem }) => {
  const hubspotAccount = useHubspotAccount({ id: integratedSystem.id });

  return (
    <div className="row text-center">
      <div className="col">
        {hubspotAccount.portalId !== 0 && (
          <div>Portal Id: {hubspotAccount.portalId || "Not Connnected"}</div>
        )}
      </div>
      <div className="col">
        <Navigation
          to={`/settings/integrations/hubspotconnector?state=${integratedSystem.id}`}
        >
          <button
            disabled={!!hubspotAccount.portalId}
            className="btn btn-primary"
          >
            Connect to Hubspot
          </button>
        </Navigation>
      </div>
    </div>
  );
};
