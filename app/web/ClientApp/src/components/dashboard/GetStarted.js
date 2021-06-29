import React from "react";
import { Link } from "react-router-dom";
import { ExpandableCard } from "../molecules/ExpandableCard";
export const GetStarted = () => {
  return (
    <div>
      <ExpandableCard label="Step 1: Connect your data.">
        <p>
          SignalBox needs data to start analysing user behaviour. Connect your
          existing data sources by:
        </p>
        <ul>
          <li>
            Start tracking your users by{" "}
            <Link to="/settings/integrations/create">
              creating a data integration
            </Link>{" "}
            or{" "}
            <Link to="/tracked-users/upload">
              uploading a .csv file with customer information.
            </Link>
          </li>
        </ul>
      </ExpandableCard>
      <ExpandableCard label="Step 2: Create some Catalogue items.">
        <ul>
          <li>
            Catalogues are offers, products, parameters, etc. that you
            optionally provide your customers.
          </li>
          <li>
            For example, define parameters to optimise in the{" "}
            <Link to="/parameters">Parameter catalog</Link>
          </li>
        </ul>
      </ExpandableCard>
      <ExpandableCard label="Step 3. Create a Recommender.">
        <ul>
          <li>
            If you created a Parameter Catalogue, you can now setup a{" "}
            <Link to="/recommenders/parameter-set-recommenders/create">
              Parameter Set Recommender.
            </Link>
          </li>
        </ul>
      </ExpandableCard>

      <ExpandableCard label="Step 4. Use your Recommender.">
        <ul>
          <li>
            Once your recommender has finished training, you can start consuming
            the recommendations in your app or CRM.
          </li>
        </ul>
      </ExpandableCard>
    </div>
  );
};
