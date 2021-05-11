import React from "react";
import { createApiKey } from "../../api/apiKeyApi";
import { useAccessToken } from "../../api-hooks/token";
import { Title } from "../molecules/PageHeadings";
import { Spinner } from "../molecules/Spinner";

export const CreateApiKey = () => {
  const [name, setName] = React.useState("");
  const [loading, setLoading] = React.useState(false);
  const [keyResponse, setKeyResponse] = React.useState();
  const token = useAccessToken();

  const handleCreate = () => {
    setLoading(true);
    createApiKey({
      success: (r) => {
        setLoading(false);
        setKeyResponse(r);
      },
      error: (e) => {
        setLoading(false);
        alert(e);
      },
      token,
      name,
    });
  };
  return (
    <React.Fragment>
      <Title>Create an API Key</Title>
      <hr />
      <div>
        <label className="form-label">Give the API Key a name.</label>
        <input
          type="text"
          className="form-control"
          placeholder="API Key Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
      </div>
      <div className="mt-2">
        <button onClick={handleCreate} className="btn btn-primary">
          Create
        </button>
      </div>
      {loading && <Spinner />}
      {keyResponse && (
        <div className="card">
          <div className="card-header">
            This API key will not be shown again.
          </div>
          <div className="card-body">{keyResponse.apiKey}</div>
        </div>
      )}
    </React.Fragment>
  );
};
