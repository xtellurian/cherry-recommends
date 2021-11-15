import React from "react";
import { useAccessToken } from "../../api-hooks/token";
import { createTenantAsync, fetchStatusAsync } from "../../api/tenantsApi";
import { useInterval } from "../../utility/useInterval";

import { AsyncButton, ErrorCard, Spinner, Title } from "../molecules";
import { NoteBox } from "../molecules/NoteBox";
import {
  TextInput,
  InputGroup,
  createLengthValidator,
  createServerErrorValidator,
  joinValidators,
} from "../molecules/TextInput";

export const CreateTenantSection = () => {
  const [status, setStatus] = React.useState();
  const token = useAccessToken();
  const [creating, setCreating] = React.useState(false);
  const [error, setError] = React.useState();
  const [nameCreated, setNameCreated] = React.useState();
  const [tenant, setTenant] = React.useState({
    name: "",
  });

  const handleCreate = () => {
    setCreating(true);
    createTenantAsync({ token, name: tenant.name })
      .then((r) => setStatus(r.status))
      .then(() => setNameCreated(tenant.name))
      .catch(setError)
      .finally(() => setCreating(false));
  };

  const updateStatus = () => {
    if (nameCreated) {
      fetchStatusAsync({ token, name: nameCreated })
        .then((r) => {
          console.log(r);
          setStatus(r.status);
        })
        .catch(setError);
    } else {
      console.log("Not checking status yet...");
    }
  };

  useInterval(() => updateStatus(), 2000);

  return (
    <div className="container">
      <Title>Create a new Tenant</Title>
      <hr />
      {error && <ErrorCard error={error} />}
      {status && (
        <div>
          <NoteBox label="Creation Status">
            <Spinner />
            <div>{status}</div>
          </NoteBox>
        </div>
      )}
      <div className="mt-3">
        <InputGroup>
          <TextInput
            disabled={creating}
            label="Tenant Name"
            placeholder="Your organisation's name"
            validator={joinValidators([
              createLengthValidator(4),
              createServerErrorValidator("Name", error),
            ])}
            value={tenant.name}
            onChange={(e) => setTenant({ ...tenant, name: e.target.value })}
          />
        </InputGroup>
      </div>
      <div className="mt-3">
        <AsyncButton
          className="btn btn-primary btn-block"
          loading={creating}
          onClick={handleCreate}
        >
          Create
        </AsyncButton>
      </div>
    </div>
  );
};
