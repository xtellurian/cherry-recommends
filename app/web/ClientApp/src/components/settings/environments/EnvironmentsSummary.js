import React from "react";
import { useEnvironments } from "../../../api-hooks/environmentsApi";
import { useAccessToken } from "../../../api-hooks/token";
import { deleteEnvironmentAsync } from "../../../api/environmentsApi";
import {
  Title,
  Subtitle,
  Spinner,
  ErrorCard,
} from "../../molecules";
import { CreateButtonClassic } from "../../molecules/CreateButton";
import { DateTimeField } from "../../molecules/DateTimeField";
import { ConfirmDeletePopup } from "../../molecules/popups/ConfirmDeletePopup";
import { CopyableField } from "../../molecules/fields/CopyableField";
import { ActiveIndicator } from "../../molecules/ActiveIndicator";

const EnvironmentRow = ({ environment }) => {
  const [isDeleteOpen, setIsDeleteOpen] = React.useState(false);
  const [error, setError] = React.useState(false);
  const token = useAccessToken();
  const handleDelete = () => {
    deleteEnvironmentAsync({
      token,
      id: environment.id,
    })
      .then(window.location.reload())
      .catch(setError);
  };
  return (
    <React.Fragment>
      <div className="row">
        <div className="col-2">
          <ActiveIndicator isActive={environment.current}>
            {environment.name}
          </ActiveIndicator>
        </div>
        <div className="col-3">
          {environment.created && (
            <DateTimeField label="Created" date={environment.created} />
          )}
        </div>
        <div className="col">
          {environment.id && (
            <CopyableField label="Environment Id" value={environment.id} />
          )}
          {environment.id === null && (
            <CopyableField label="Environment Id" value="null" />
          )}
        </div>
        <div className="col-2">
          {environment.id && (
            <button
              className="btn btn-danger btn-block"
              onClick={() => setIsDeleteOpen(true)}
            >
              Delete
            </button>
          )}
        </div>
      </div>

      <ConfirmDeletePopup
        entity={environment}
        open={isDeleteOpen}
        setOpen={setIsDeleteOpen}
        error={error}
        handleDelete={handleDelete}
      />
    </React.Fragment>
  );
};
export const EnvironmentsSummary = () => {
  const environments = useEnvironments();
  return (
    <React.Fragment>
      <CreateButtonClassic className="float-right" to="/settings/environments/create">
        Create environment
      </CreateButtonClassic>
      <Title>Environments</Title>
      <Subtitle></Subtitle>

      <hr />

      {environments.loading && <Spinner />}
      {environments.error && <ErrorCard error={environments.error} />}

      {environments.items &&
        environments.items.map((e) => (
          <EnvironmentRow key={e.id} environment={e} />
        ))}
    </React.Fragment>
  );
};
