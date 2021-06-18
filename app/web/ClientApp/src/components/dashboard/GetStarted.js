import React from "react";
import { Link } from "react-router-dom";

export const GetStarted = () => {
  return (
    <div>
      <h6>Step 1: Connect your data.</h6>
      <p>
        SignalBox needs data to start analysing user behaviour. Connect your
        existing data sources by:
      </p>
      <ul>
        <li>
          <Link to="/settings/integrations/create">
            Creating a data integration
          </Link>
        </li>
        <li>
          <Link to="/tracked-users/upload">
            Uploading a .csv file with customer information.
          </Link>
        </li>
      </ul>
    </div>
  );
};
