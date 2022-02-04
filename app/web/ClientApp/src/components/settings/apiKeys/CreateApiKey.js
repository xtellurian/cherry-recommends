import React from "react";
import { createApiKeyAsync } from "../../../api/apiKeyApi";
import { useAccessToken } from "../../../api-hooks/token";
import { Title, Spinner, ErrorCard } from "../../molecules";
import { Selector } from "../../molecules/selectors/Select";
import { InputGroup, TextInput } from "../../molecules/TextInput";
import { useAnalytics } from "../../../analytics/analyticsHooks";

const options = [
  {
    label: "Server",
    value: "Server",
  },
  {
    label: "Web",
    value: "Web",
  },
];
export const CreateApiKey = () => {
  const { analytics } = useAnalytics();
  const [name, setName] = React.useState("");
  const [loading, setLoading] = React.useState(false);
  const [error, setError] = React.useState(false);
  const [keyResponse, setKeyResponse] = React.useState();
  const token = useAccessToken();
  const [selectedOption, setSelectedOption] = React.useState(options[0]);

  const handleCreate = () => {
    setError(null);
    setLoading(true);
    createApiKeyAsync({
      token,
      payload: {
        name,
        apiKeyType: selectedOption.value,
      },
    })
      .then((r) => {
        analytics.track("site:settings_apiKey_create_success");
        setKeyResponse(r);
      })
      .catch((e) => {
        analytics.track("site:settings_apiKey_create_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };

  return (
    <React.Fragment>
      <Title>Create an API Key</Title>
      <hr />
      {error && <ErrorCard error={error} />}
      <div>
        <InputGroup>
          <TextInput
            label="Name"
            placeholder="API Key Name"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
          <Selector
            className="w-25"
            placeholder="Api Key Type"
            defaultValue={selectedOption}
            onChange={setSelectedOption}
            options={options}
          />
        </InputGroup>
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
