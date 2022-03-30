import React from "react";
import { Subtitle, Title, ErrorCard, Spinner, EmptyList } from "../molecules";
import { useParameters } from "../../api-hooks/parametersApi";
import { ParameterRow } from "./ParameterRow";
import { CreateButtonClassic } from "../molecules/CreateButton";

export const ParametersSummary = () => {
  const [created, setCreated] = React.useState();
  const [deleted, setDeleted] = React.useState();
  const parameters = useParameters({ trigger: created || deleted });

  return (
    <React.Fragment>
      <CreateButtonClassic className="float-right" to="/parameters/create">
        Create Parameter
      </CreateButtonClassic>
      <Title>Parameters</Title>
      <hr />

      {parameters.error && <ErrorCard error={parameters.error} />}
      {parameters.loading && <Spinner />}
      {parameters.items && parameters.items.length === 0 && (
        <EmptyList>No Parameters</EmptyList>
      )}
      {parameters &&
        parameters.items &&
        parameters.items.map((p) => (
          <ParameterRow key={p.id} parameter={p} onDeleted={setDeleted} />
        ))}
    </React.Fragment>
  );
};
