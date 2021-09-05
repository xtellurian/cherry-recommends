import React from "react";
import { CreateParameterPanel } from "./CreateParameter";
import { Subtitle, Title, ErrorCard, Spinner, EmptyList } from "../molecules";

import { useParameters } from "../../api-hooks/parametersApi";
import { deleteParameterAsync } from "../../api/parametersApi";
import { ExpandableCard } from "../molecules/ExpandableCard";
import { CopyableField } from "../molecules/fields/CopyableField";
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";
import { useAccessToken } from "../../api-hooks/token";

const ParameterRow = ({ parameter, onDeleted }) => {
  const token = useAccessToken();
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [error, setError] = React.useState();
  const handleDelete = () => {
    deleteParameterAsync({ token, id: parameter.id })
      .then(onDeleted)
      .catch(setError);
  };
  return (
    <div className="row">
      <div className="col">
        <ExpandableCard
          label={`${parameter.name} (${parameter.parameterType})`}
        >
          <div>
            <CopyableField label="Identifier" value={parameter.commonId} />
            <CopyableField
              label="Default Value"
              value={parameter.default || "null"}
            />
            <p>{parameter.description}</p>
          </div>
          <button
            className="btn btn-danger"
            onClick={() => setDeleteOpen(true)}
          >
            Delete Parameter
          </button>

          <ConfirmDeletePopup
            entity={parameter}
            open={deleteOpen}
            setOpen={setDeleteOpen}
            error={error}
            handleDelete={handleDelete}
          />
        </ExpandableCard>
      </div>
    </div>
  );
};

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
