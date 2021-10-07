import React from "react";
import { CreateParameterPanel } from "./CreateParameter";
import { Subtitle, Title, ErrorCard, Spinner, EmptyList } from "../molecules";
import { useParameters } from "../../api-hooks/parametersApi";
import { ParameterRow } from "./ParameterRow";

export const ParametersSummary = () => {
  const [created, setCreated] = React.useState();
  const [deleted, setDeleted] = React.useState();
  const parameters = useParameters({ trigger: created || deleted });

  return (
    <React.Fragment>
      <Title>Parameters</Title>
      <hr />

      <div className="row">
        <div className="col">
          <Subtitle>Existing Parameters </Subtitle>
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
        </div>
        <div className="col">
          <CreateParameterPanel onCreated={setCreated} />
        </div>
      </div>
    </React.Fragment>
  );
};
