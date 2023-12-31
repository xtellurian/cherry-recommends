import React from "react";
import { Link } from "react-router-dom";
import { ExpandableCard } from "../molecules/ExpandableCard";
import { Navigation } from "../molecules";
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
            <Navigation to="/settings/integrations/create">
              creating a data integration
            </Navigation>{" "}
            or{" "}
            <Navigation to="/customers/customers/upload">
              uploading a .csv file with customer information.
            </Navigation>
          </li>
        </ul>
      </ExpandableCard>
      <ExpandableCard label="Step 2: Create some Catalogue items.">
        <ul>
          <li>
            Catalogues are offers, items, parameters, etc. that you optionally
            provide your customers.
          </li>
          <li>
            For example, define parameters to optimise in the{" "}
            <Navigation to="/parameters/parameters">
              Parameter catalog
            </Navigation>
          </li>
        </ul>
      </ExpandableCard>
      <ExpandableCard label="Step 3. Create a Campaign.">
        <ul>
          <li>
            If you created a Parameter Catalogue, you can now setup a{" "}
            <Navigation to="/campaigns/parameter-set-campaigns/create">
              Parameter Set Campaign.
            </Navigation>
          </li>
        </ul>
      </ExpandableCard>

      <ExpandableCard label="Step 4. Use your Campaign.">
        <ul>
          <li>
            Once your campaign has finished training, you can start consuming
            the recommendations in your app or CRM.
          </li>
        </ul>
      </ExpandableCard>
    </div>
  );
};
