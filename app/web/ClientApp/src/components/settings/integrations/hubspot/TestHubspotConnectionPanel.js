import React from "react";
import { Link } from "react-router-dom";
import { Spinner } from "../../../molecules/Spinner";

export const TestHubspotConnectionPanel = ({ integratedSystem }) => {
  if (integratedSystem.loading) {
    return <Spinner />;
  }
  if (integratedSystem.systemType === "hubspot") {
    return (
      <React.Fragment>
        <Link
          to={`/settings/integrations/testhubspotconnection/${integratedSystem.id}`}
        >
          <button className="btn btn-outline-primary">Test Connection</button>
        </Link>
      </React.Fragment>
    );
  } else {
    return <div className="text-muted">Testing unavailable for Segment</div>;
  }
};
