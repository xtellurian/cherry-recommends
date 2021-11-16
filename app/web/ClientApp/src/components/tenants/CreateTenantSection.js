import React from "react";
import { useAccessToken } from "../../api-hooks/token";
import { createTenantAsync, fetchStatusAsync } from "../../api/tenantsApi";
import { useInterval } from "../../utility/useInterval";
import { useHosting } from "../../api-hooks/tenantsApi";
import { AsyncButton, ErrorCard, Spinner, Title } from "../molecules";
import { BigPopup } from "../molecules/popups/BigPopup";
import { Markdown } from "../molecules/Markdown";
import { NoteBox } from "../molecules/NoteBox";
import { ButtonGroup } from "../molecules/buttons/ButtonGroup";
import {
  TextInput,
  InputGroup,
  createLengthValidator,
  createServerErrorValidator,
  joinValidators,
  lowercaseOnlyValidator,
  createServerNameUnavailableValidator,
} from "../molecules/TextInput";

const nameRequirements = [
  "At least 4 characters",
  "Letters and numbers only",
  "No special characters, except underscore or hyphen",
  "Must start with a letter",
];

const termsVersion = "v1";

export const CreateTenantSection = () => {
  const [status, setStatus] = React.useState();
  const token = useAccessToken();
  const [creating, setCreating] = React.useState(false);
  const [error, setError] = React.useState();
  const [nameCreated, setNameCreated] = React.useState();
  const [termsPopupOpen, setTermsPopupOpen] = React.useState(false);
  const [termsOfService, setTermsOfService] = React.useState();
  const [serverDryRun, setServerDryRun] = React.useState();
  const hosting = useHosting();

  const [tenant, setTenant] = React.useState({
    name: "",
    termsOfServiceVersion: null,
  });

  const isLongEnoughName = tenant?.name?.length > 3;

  React.useEffect(() => {
    if (!termsOfService) {
      fetch(`/terms/${termsVersion}.md`)
        .then((response) => response.text())
        .then(setTermsOfService)
        .catch((e) => {
          console.log("Failed to fetch terms");
          console.log(e);
        });
    }
  }, []);

  React.useEffect(() => {
    if (tenant.name && isLongEnoughName) {
      createTenantAsync({
        token,
        ...tenant,
        termsOfServiceVersion: "dryrun",
        dryRun: true,
      })
        .then(() => setServerDryRun(null))
        .catch(setServerDryRun);
    }
  }, [tenant]);

  const handleCreate = (termsOfServiceVersion) => {
    setCreating(true);
    if (termsOfServiceVersion) {
      setTermsPopupOpen(false);
      createTenantAsync({ token, ...tenant, termsOfServiceVersion })
        .then((r) => setStatus(r.status))
        .then(() => setNameCreated(tenant.name))
        .catch(setError)
        .finally(() => setCreating(false));
    } else {
      setTermsPopupOpen(true);
    }
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

  return (
    <div className="container">
      <BigPopup isOpen={termsPopupOpen} setIsOpen={setTermsPopupOpen}>
        <div>
          <div className="overflow-auto" style={{ maxHeight: "75vh" }}>
            <Markdown>{termsOfService}</Markdown>
          </div>
          <div>
            <ButtonGroup className="w-100 mt-2">
              <button
                onClick={() => {
                  setTermsPopupOpen(false);
                  setCreating(false);
                  setTenant({ ...tenant, termsOfServiceVersion: null });
                }}
                className="btn btn-outline-danger"
              >
                Decline
              </button>
              <button
                onClick={() => {
                  setTenant({ ...tenant, termsOfServiceVersion: termsVersion });
                  if (creating) {
                    handleCreate(termsVersion); // call create again, because we must have been popup during create process
                  }
                  setTermsPopupOpen(false);
                }}
                className="btn btn-primary"
              >
                Accept
              </button>
            </ButtonGroup>
          </div>
        </div>
      </BigPopup>
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
                <Spinner>{status}</Spinner>
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
                  lowercaseOnlyValidator,
                  createServerNameUnavailableValidator(serverDryRun),
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
              onClick={() => handleCreate(tenant.termsOfServiceVersion)}
            >
              Create
            </AsyncButton>
          </div>
          <div className="text-center text-small">
            <button
              className="btn btn-link"
              onClick={() => setTermsPopupOpen(true)}
            >
              View the Terms of Service
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
