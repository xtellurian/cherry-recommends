import React from "react";
import { Link } from "react-router-dom";
import { useHubspotAccount } from "../../../../api-hooks/hubspotApi";

export const HubspotDetail = ({ integratedSystem }) => {
  const hubspotAccount = useHubspotAccount({ id: integratedSystem.id });

  return (
    <div className="row text-center">
      <div className="col">
        {hubspotAccount.portalId !== 0 && (
          <div>Portal Id: {hubspotAccount.portalId}</div>
        )}
      </div>
      <div className="col">
        <Link to={`/settings/integrations/hubspot-detail/${integratedSystem.id}`}>
          <button className="btn btn-primary">
            Details
          </button>
        </Link>
        {/* <HubspotContactProperties integratedSystem={integratedSystem} /> */}
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
