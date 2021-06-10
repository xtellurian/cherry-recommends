import React from "react";
import { useApiKeys } from "../../../api-hooks/apiKeyApi";
import { Title } from "../../molecules/PageHeadings";
import { CreateButton } from "../../molecules/CreateButton";
import { Spinner } from "../../molecules/Spinner";

const ApiKeyRow = ({ id, name, lastExchanged, totalExchanges }) => {
  return (
    <div className="card">
      <div className="row card-body">
        <div className="col-1">{id}</div>
        <div className="col">{name}</div>
        <div className="col">Exchanged {lastExchanged || "Never"}</div>
        <div className="col-1">{totalExchanges}</div>
      </div>
    </div>
  );
};
const Top = () => {
  return (
    <React.Fragment>
      <CreateButton to="/settings/api-keys/create" className="float-right">
        New API key
      </CreateButton>
      <Title>API keys</Title>
      <hr />
    </React.Fragment>
  );
};
export const ListApiKeys = () => {
  const { result } = useApiKeys();
  if (result && result.loading) {
    return (
      <React.Fragment>
        <Top />
        <Spinner>Fetching API Keys</Spinner>
      </React.Fragment>
    );
  }
  return (
    <React.Fragment>
      <Top/>

      <div>
        <ul>
          {result &&
            result.items &&
            result.items.map((item, j) => (
              <ApiKeyRow key={item.id} {...item}></ApiKeyRow>
            ))}
        </ul>
      </div>
      {result && result.items.length === 0 && <div>No API Keys</div>}
    </React.Fragment>
  );
};
