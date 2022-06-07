import React from "react";
import { useAnalytics } from "../../analytics/analyticsHooks";
import { useAccessToken } from "../../api-hooks/token";
import { createTenantAsync, fetchStatusAsync } from "../../api/tenantsApi";
import { useInterval } from "../../utility/useInterval";
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

import { PersonalizationCarousel } from "./PersonalizationCarousel";
import { Progress } from "../molecules/Progress";
import { useNavigation } from "../../utility/useNavigation";

const nameRequirements = [
  "At least 4 characters",
  "Lowercase letters and numbers only",
  "No special characters, except underscore or hyphen",
  "Must start with a letter, not a number or symbol",
];

const termsVersion = "v1";

const toFriendlyStatus = (status) => {
  if (status === "Not Exist") {
    return "Preparing Seedlings";
  } else if (status === "Submitted") {
    return "Sewing Seeds";
  } else if (status === "Creating") {
    return "Fertilising Soils";
  } else if (status === "DatabaseCreated") {
    return "Growing Branches";
  } else if (status === "Created") {
    return "Maturing Trees";
  } else {
    console.warn(`Unknown status: ${status}`);
    return "Lost Leaves";
  }
};

export const CreateTenantSection = () => {
  const [status, setStatus] = React.useState();
  const token = useAccessToken();
  const [creating, setCreating] = React.useState(false);
  const [error, setError] = React.useState();
  const [nameCreated, setNameCreated] = React.useState();
  const [termsPopupOpen, setTermsPopupOpen] = React.useState(false);
  const [termsOfService, setTermsOfService] = React.useState();
  const [serverDryRun, setServerDryRun] = React.useState();
  const { analytics } = useAnalytics();

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
          console.error("Failed to fetch terms");
          console.error(e);
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
        .then(() => {
          analytics.track("site:tenant_create_success");
          setNameCreated(tenant.name);
        })
        .catch((e) => {
          analytics.track("site:tenant_create_failure");
          setError(e);
        })
        .finally(() => setCreating(false));
    } else {
      setTermsPopupOpen(true);
    }
  };

  const updateStatus = () => {
    if (nameCreated) {
      fetchStatusAsync({ token, name: nameCreated })
        .then((r) => {
          console.info(r);
          setStatus(r.status);
        })
        .catch(setError);
    } else {
      console.debug("Not checking status yet...");
    }
  };

  useInterval(() => updateStatus(), 5000);
  const { navigate } = useNavigation();
  React.useEffect(() => {
    if (status === "Created") {
      navigate({ pathname: `/${nameCreated}` });
    }
  }, [status]);

  if (status) {
    return (
      <div className="container">
        <div className="text-center mt-5">
          <Title>Create a new Tenant</Title>
        </div>
        <div className="w-50 mx-auto mt-4">
          <div className="card">
            <PersonalizationCarousel />
          </div>
          <div className="mt-5 text-muted text-center">
            <div className="mb-1">Please wait a moment</div>
            <Progress seconds={80}>{toFriendlyStatus(status)}</Progress>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="container">
      <BigPopup
        isOpen={termsPopupOpen}
        setIsOpen={setTermsPopupOpen}
        buttons={
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
        }
      >
        <div>
          <div className="overflow-auto" style={{ maxHeight: "75vh" }}>
            <Markdown>{termsOfService}</Markdown>
          </div>
        </div>
      </BigPopup>
      <div className="mt-5">
        <div className="text-center">
          <Title>Create a new Tenant</Title>
          <span className="text-bold">
            Your tenant is the home for all your business' promotions.
          </span>
        </div>
        <div className="w-50 m-auto">
          {error && <ErrorCard error={error} />}
          {status && (
            <div>
              <NoteBox label="Creation Status">
                <Spinner>{toFriendlyStatus(status)}</Spinner>
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
                  {`https://${window.location.host}/${tenant.name}`}
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
