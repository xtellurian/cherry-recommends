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
          Start tracking your users by{" "}
          <Link to="/settings/integrations/create">
            creating a data integration
          </Link>
        </li>
        <li>
          Explore the <Link to="/docs/api">API Docs</Link>
        </li>
        <li>
          <Link to="/tracked-users/upload">
            Uploading a .csv file with customer information.
          </Link>
        </li>
      </ul>
      <h6>Step 2: Create some catalog.</h6>
      <ul>
        <li>
          Define parameters to optimise in the{" "}
          <Link to="/parameters">Parameter catalog</Link>
        </li>
        {/* <li>
          Upload your products to the{" "}
          <Link to="/products">Product catalog</Link>
        </li> */}
      </ul>
    </div>
  );
};
