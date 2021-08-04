import React from "react";
import { Link } from "react-router-dom";
import { useHubspotAccount } from "../../../../api-hooks/hubspotApi";

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
        <Link
          to={`/settings/integrations/hubspotconnector?state=${integratedSystem.id}`}
        >
          <button
            disabled={!!hubspotAccount.portalId}
            className="btn btn-primary"
          >
            Connect to Hubspot
          </button>
        </Link>
      </div>
    </div>
  );
};
