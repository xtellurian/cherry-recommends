import React from "react";
import { useIntegratedSystems } from "../../../api-hooks/integratedSystemsApi";
import { Title } from "../../molecules/PageHeadings";
import { CreateButton } from "../../molecules/CreateButton";
import { Spinner } from "../../molecules/Spinner";
import { ErrorCard } from "../../molecules/ErrorCard";
import { EmptyList } from "../../molecules/EmptyList";
import { Link } from "react-router-dom";

const IntegrationRow = ({ integration }) => {
  return (
    <div className="card">
      <div className="row card-body">
        <div className="col">
          <h5>{integration.name}</h5>
        </div>
        <div className="col-2 text-center">{integration.systemType}</div>
        <div className="col-2 text-right">
          <Link to={`/settings/integrations/detail/${integration.id}`}>
            <button className="btn btn-outline-primary">Detail</button>
          </Link>
        </div>
      </div>
    </div>
  );
};
export const ListIntegrations = () => {
  const result = useIntegratedSystems();

  if (result.error) {
    return (
      <ErrorCard error={result.error} />
    );
  }
  return (
    <React.Fragment>
      <div>
        <CreateButton
          to="/settings/integrations/create"
          className="float-right"
        >
          Create Integration
        </CreateButton>
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
            <CreateButton to="/settings/integrations/create">
              Create Integrated System
            </CreateButton>
          </EmptyList>
        )}
      </div>
    </React.Fragment>
  );
};
