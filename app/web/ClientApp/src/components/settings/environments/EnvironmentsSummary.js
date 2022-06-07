import React from "react";
import {
  useEnvironmentReducer,
  useEnvironments,
} from "../../../api-hooks/environmentsApi";
import { useAccessToken } from "../../../api-hooks/token";
import { deleteEnvironmentAsync } from "../../../api/environmentsApi";
import { Spinner } from "../../molecules";
import { DateTimeField } from "../../molecules/DateTimeField";
import { ConfirmDeletePopup } from "../../molecules/popups/ConfirmDeletePopup";
import { CopyableField } from "../../molecules/fields/CopyableField";
import { ActiveIndicator } from "../../molecules/ActiveIndicator";

import Layout, {
  CreateEntityButton,
} from "../../molecules/layout/EntitySummaryLayout";

const EnvironmentRow = ({ environment, reload }) => {
  const [currentEnviroment, setEnvironment] = useEnvironmentReducer();
  const [isDeleteOpen, setIsDeleteOpen] = React.useState(false);
  const [error, setError] = React.useState(false);
  const token = useAccessToken();
  const handleDelete = () => {
    deleteEnvironmentAsync({
      token,
      id: environment.id,
    })
      .then(() => {
        if (environment.current) {
          setEnvironment(null);
          console.debug("reset env");
        } else {
          console.debug(`Staying on environment ${currentEnviroment?.id}`);
        }
        reload();
      })
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
        extraErrorText=" Delete may fail if your environment contains unexpected data. Contact
        support to continue."
      />
    </React.Fragment>
  );
};
export const EnvironmentsSummary = () => {
  const [trigger, setTrigger] = React.useState({});
  const environments = useEnvironments({ trigger });
  return (
    <Layout
      header="Environments"
      createButton={
        <CreateEntityButton to="/settings/environments/create">
          Create an Environment
        </CreateEntityButton>
      }
      error={environments.error}
    >
      {environments.loading && <Spinner />}

      {environments.items &&
        environments.items.map((e) => (
          <EnvironmentRow
            key={e.id}
            environment={e}
            reload={() => setTrigger({})}
          />
        ))}
    </Layout>
  );
};
