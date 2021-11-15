import React from "react";
import { useAccessToken } from "../../api-hooks/token";
import { createTenantAsync, fetchStatusAsync } from "../../api/tenantsApi";
import { useInterval } from "../../utility/useInterval";
import { useHosting } from "../../api-hooks/tenantsApi";
import { AsyncButton, ErrorCard, Spinner, Subtitle, Title } from "../molecules";
import { NoteBox } from "../molecules/NoteBox";
import {
  TextInput,
  InputGroup,
  createLengthValidator,
  createServerErrorValidator,
  joinValidators,
} from "../molecules/TextInput";

const nameRequirements = [
  "At least 4 characters",
  "Letters and numbers only",
  "No special characters, except underscore or hyphen",
  "Must start with a letter",
];

export const CreateTenantSection = () => {
  const [status, setStatus] = React.useState();
  const token = useAccessToken();
  const [creating, setCreating] = React.useState(false);
  const [error, setError] = React.useState();
  const [nameCreated, setNameCreated] = React.useState();
  const hosting = useHosting();

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

  useInterval(() => updateStatus(), 5000);

  React.useEffect(() => {
    if (status === "Created") {
      window.location = `https://${nameCreated}.${hosting.canonicalRootDomain}?autoSignIn=true`;
    }
  }, [status]);

  const isLongEnoughName = tenant?.name?.length > 3;

  return (
    <div className="container">
      <div className="mt-5">
        <div className="text-center">
          <Title>Create a new Tenant</Title>
          <span className="text-bold">
            Your tenant is the home of all your customers' data
          </span>
        </div>
        <div className="w-50 m-auto">
          {error && <ErrorCard error={error} />}
          {status && (
            <div>
              <NoteBox label="Creation Status">
                <Spinner />
                <div>{status}</div>
              </NoteBox>
            </div>
          )}
          <div className="mt-3 mb-3">
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

            <div className="text-muted">
              {isLongEnoughName && (
                <small>
                  Your tenant will be available at{" "}
                  {`https://${tenant.name}.${hosting.canonicalRootDomain}`}
                </small>
              )}
              {!isLongEnoughName && <small>.</small>}
            </div>
            <div className="m-3 text-muted">
              <p>Create a name using - </p>
              <ul>
                {nameRequirements.map((r) => (
                  <li key={r}>{r}</li>
                ))}
              </ul>
            </div>
          </div>
          <div className="w-50 m-auto">
            <AsyncButton
              disabled={!isLongEnoughName}
              className="btn btn-primary btn-block"
              loading={creating}
              onClick={handleCreate}
            >
              Create
            </AsyncButton>
          </div>
        </div>
      </div>
    </div>
  );
};
