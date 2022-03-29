import React from "react";

import { useIntegratedSystems } from "../../../api-hooks/integratedSystemsApi";
import { Title } from "../../molecules/layout";
import { Navigation } from "../../molecules";
import { CreateButtonClassic } from "../../molecules/CreateButton";
import { Spinner } from "../../molecules/Spinner";
import { ErrorCard } from "../../molecules/ErrorCard";
import { EmptyList } from "../../molecules/";
import { IntegrationIcon } from "./icons/IntegrationIcons";

const IntegrationRow = ({ integration }) => {
  return (
    <div className="card">
      <div className="row card-body">
        <div className="col">
          <h5>{integration.name}</h5>
        </div>
        <div className="col-2 text-center text-capitalize">
          {integration.systemType}
        </div>
        <div className="col-1 text-left">
          <IntegrationIcon integration={integration} />
        </div>
        <div className="col-2 text-right">
          <Navigation to={`/settings/integrations/detail/${integration.id}`}>
            <button className="btn btn-outline-primary">Detail</button>
          </Navigation>
        </div>
      </div>
    </div>
  );
};
export const ListIntegrations = () => {
  const result = useIntegratedSystems();

  if (result.error) {
    return <ErrorCard error={result.error} />;
  }
  return (
    <React.Fragment>
      <div>
        <CreateButtonClassic
          to="/settings/integrations/create"
          className="float-right"
        >
          Create Integration
        </CreateButtonClassic>
        <Title>Integrations</Title>
        <hr />
        {result.loading && <Spinner />}
        {result.items &&
          result.items.length > 0 &&
          result.items.map((i) => (
            <IntegrationRow key={i.id} integration={i} />
          ))}

        {result && result.items && result.items.length === 0 && (
          <EmptyList>
            No Integrated Systems.
            <CreateButtonClassic to="/settings/integrations/create">
              Create Integrated System
            </CreateButtonClassic>
          </EmptyList>
        )}
      </div>
    </React.Fragment>
  );
};
