import React from "react";
import { Link } from "react-router-dom";

export const GetStarted = () => {
  return (
    <div>
      <h6>Step 1: Connect your data.</h6>
      <p>
        We need data to start analysing your customers. Connect your existing
        data sources by:
      </p>
      <ul>
        <li>
          <Link to="/settings/integrations/create">
            Creating a Segment integration
          </Link>
        </li>
        <li>
          <Link to="/tracked-users/upload">
            Uploading a .csv file with customer information.
          </Link>
        </li>
      </ul>

      <h6>Step 2. Create your archetypes.</h6>
      <h6>Step 3. Choose a health score.</h6>
    </div>
  );
};
