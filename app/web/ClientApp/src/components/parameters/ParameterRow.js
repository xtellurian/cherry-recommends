import React from "react";
import { deleteParameterAsync } from "../../api/parametersApi";
import { ExpandableCard } from "../molecules/ExpandableCard";
import { CopyableField } from "../molecules/fields/CopyableField";
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";
import { useAccessToken } from "../../api-hooks/token";

export const ParameterRow = ({ parameter, disableDelete, onDeleted }) => {
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
          {!disableDelete && (
            <React.Fragment>
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
            </React.Fragment>
          )}
        </ExpandableCard>
      </div>
    </div>
  );
};
