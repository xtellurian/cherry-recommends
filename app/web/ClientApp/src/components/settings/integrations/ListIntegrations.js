import React from "react";
import { useIntegratedSystems } from "../../../api-hooks/integratedSystemsApi";
import { Title } from "../../molecules/PageHeadings";
import { CreateButton } from "../../molecules/CreateButton";
import { Spinner } from "../../molecules/Spinner";
export const ListIntegrations = () => {
  const { integratedSystems, error, loading } = useIntegratedSystems();

  if(error) {
      alert(error)
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
        {loading && <Spinner />}
        {integratedSystems &&
          integratedSystems.map((i) => <div key={i.id}>{i.name}</div>)}

        {integratedSystems && integratedSystems.length === 0 && (
          <div>No Systems Yet.</div>
        )}
      </div>
    </React.Fragment>
  );
};
