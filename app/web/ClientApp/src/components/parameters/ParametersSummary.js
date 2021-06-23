import React from "react";
import { CreateParameterPanel } from "./CreateParameter";
import { Subtitle, Title } from "../molecules/PageHeadings";
import { ErrorCard } from "../molecules/ErrorCard";
import { Spinner } from "../molecules/Spinner";
import { EmptyList } from "../molecules/EmptyList";
import { useParameters } from "../../api-hooks/parametersApi";
import { ExpandableCard } from "../molecules/ExpandableCard";
import { CopyableField } from "../molecules/CopyableField";

const ParameterRow = ({ parameter }) => {
  return (
    <div className="row">
      <div className="col">
        <ExpandableCard name={`${parameter.name} (${parameter.parameterType})`}>
          <div>
            <CopyableField label="Identifier" value={parameter.commonId} />
            <p>{parameter.description}</p>
          </div>
        </ExpandableCard>
      </div>
    </div>
  );
};

export const ParametersSummary = () => {
  const [created, setCreated] = React.useState();
  const parameters = useParameters({ trigger: created });

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
              <ParameterRow key={p.id} parameter={p} />
            ))}
        </div>
        <div className="col">
          <CreateParameterPanel onCreated={setCreated} />
        </div>
      </div>
    </React.Fragment>
  );
};
