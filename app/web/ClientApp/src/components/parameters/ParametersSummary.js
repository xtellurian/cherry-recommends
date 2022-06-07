import React from "react";
import { Spinner, EmptyList } from "../molecules";
import { useParameters } from "../../api-hooks/parametersApi";
import { ParameterRow } from "./ParameterRow";

import Layout, {
  CreateEntityButton,
} from "../molecules/layout/EntitySummaryLayout";

export const ParametersSummary = () => {
  const [created, setCreated] = React.useState();
  const [deleted, setDeleted] = React.useState();
  const parameters = useParameters({ trigger: created || deleted });

  return (
    <Layout
      header="Parameters"
      createButton={
        <CreateEntityButton to="/parameters/parameters/create">
          Create a Parameter
        </CreateEntityButton>
      }
      error={parameters.error}
    >
      {parameters.loading && <Spinner />}
      {parameters.items && parameters.items.length === 0 && (
        <EmptyList>No Parameters</EmptyList>
      )}
      {parameters &&
        parameters.items &&
        parameters.items.map((p) => (
          <ParameterRow key={p.id} parameter={p} onDeleted={setDeleted} />
        ))}
    </Layout>
  );
};
