import React from "react";
import { useApiKeys } from "../../../api-hooks/apiKeyApi";
import { Title } from "../../molecules/PageHeadings";
import { CreateButton } from "../../molecules/CreateButton";

export const ListApiKeys = () => {
  const { apiKeys } = useApiKeys();
  console.log(apiKeys);
  return (
    <React.Fragment>
      <CreateButton to="/settings/api-keys/create" className="float-right">
          New API key
      </CreateButton>
      <Title>API keys</Title>
      <hr/>
      <div>
        <ul>{apiKeys && apiKeys.map((name, j) => <li key={j}> {name}</li>)}</ul>
      </div>
      {apiKeys && apiKeys.length === 0 && <div>No API Keys</div>}
    </React.Fragment>
  );
};
