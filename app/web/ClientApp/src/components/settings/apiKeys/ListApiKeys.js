import React from "react";
import { deleteApiKeyAsync } from "../../../api/apiKeyApi";
import { useApiKeys } from "../../../api-hooks/apiKeyApi";
import { Title } from "../../molecules/layout";
import { CreateButtonClassic } from "../../molecules/CreateButton";
import { Spinner } from "../../molecules/Spinner";
import { ErrorCard } from "../../molecules";
import { ConfirmDeletePopup } from "../../molecules/popups/ConfirmDeletePopup";
import { useAccessToken } from "../../../api-hooks/token";

const ApiKeyRow = ({ apiKey, onDelete, onDeleteError }) => {
  const token = useAccessToken();
  const handleDelete = () => {
    deleteApiKeyAsync({ id: apiKey.id, token })
      .then(onDelete)
      .catch(onDeleteError);
  };
  const [isDeleteOpen, setDeleteOpen] = React.useState(false);

  return (
    <React.Fragment>
      <div className="card">
        <div className="row justify-content-between card-body">
          <div className="col-2 font-weight-bold">{apiKey.apiKeyType}</div>
          <div className="col">{apiKey.name}</div>
          <div className="col">Exchanged {apiKey.lastExchanged || "Never"}</div>
          <div className="col-2">Exchanges: {apiKey.totalExchanges}</div>
          <div className="col">
            <button
              className="btn btn-danger btn-block"
              onClick={() => setDeleteOpen(true)}
            >
              Delete
            </button>
          </div>
        </div>
      </div>
      <ConfirmDeletePopup
        entity={apiKey}
        open={isDeleteOpen}
        setOpen={setDeleteOpen}
        handleDelete={handleDelete}
      />
    </React.Fragment>
  );
};
const Top = () => {
  return (
    <React.Fragment>
      <CreateButtonClassic
        to="/settings/api-keys/create"
        className="float-right"
      >
        New API key
      </CreateButtonClassic>
      <Title>API keys</Title>
      <hr />
    </React.Fragment>
  );
};
export const ListApiKeys = () => {
  const [trigger, setTrigger] = React.useState();
  const [error, setError] = React.useState();
  const { result } = useApiKeys({ trigger });
  if (result && result.loading) {
    return (
      <React.Fragment>
        <Top />
        <Spinner>Fetching API Keys</Spinner>
      </React.Fragment>
    );
  }
  console.log(result);
  return (
    <React.Fragment>
      <Top />
      {error && <ErrorCard error={error} />}
      <div>
        <ul>
          {result &&
            result.items &&
            result.items.map((item, j) => (
              <ApiKeyRow
                key={item.id}
                apiKey={item}
                onDelete={setTrigger}
                onDeleteError={setError}
              ></ApiKeyRow>
            ))}
        </ul>
      </div>
      {result && result.items.length === 0 && <div>No API Keys</div>}
    </React.Fragment>
  );
};
