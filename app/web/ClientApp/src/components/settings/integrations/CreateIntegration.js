import React from "react";

import { useAnalytics } from "../../../analytics/analyticsHooks";
import { useAccessToken } from "../../../api-hooks/token";
import { createIntegratedSystemAsync } from "../../../api/integratedSystemsApi";
import { ErrorCard } from "../../molecules/ErrorCard";
import { AsyncButton } from "../../molecules/AsyncButton";
import { NoteBox } from "../../molecules/NoteBox";
import { TextInput, InputGroup } from "../../molecules/TextInput";
import { IntegrationIcon } from "./icons/IntegrationIcons";
import { MoveUpHierarchyPrimaryButton, PageHeading } from "../../molecules";
import { useNavigation } from "../../../utility/useNavigation";

const systemTypes = ["Shopify", "Hubspot", "Segment", "Custom", "Website"];

const ShopifyNextSteps = () => {
  return (
    <React.Fragment>
      <div>
        <p>
          The Cherry Shopify app is currently under review. It will become
          available on the Shopify App Store shortly.
        </p>
        <a href="https://apps.shopify.com/">
          <button className="btn btn-primary">View on Shopify App Store</button>
        </a>
      </div>
    </React.Fragment>
  );
};

const GenericNextSteps = ({
  handleCreate,
  integratedSystem,
  setIntegratedSystem,
  creating = { creating },
}) => {
  return (
    <React.Fragment>
      <div className="row justify-content-center align-items-center">
        <div className="col-3 text-capitalize">
          <h3>{integratedSystem.systemType}</h3>
        </div>
        <div className="col-2">
          <IntegrationIcon systemType={integratedSystem.systemType} />
        </div>
      </div>
      <InputGroup className="mt-2 mb-2">
        <TextInput
          placeholder="System Name"
          label="What name should we give the external system?"
          value={integratedSystem.name}
          onChange={(e) =>
            setIntegratedSystem({
              ...integratedSystem,
              name: e.target.value,
            })
          }
        />
      </InputGroup>

      <AsyncButton
        loading={creating}
        disabled={!integratedSystem.name || integratedSystem.name.length < 3}
        className="btn btn-primary btn-block mt-3"
        onClick={handleCreate}
      >
        Create
      </AsyncButton>
    </React.Fragment>
  );
};
export const CreateIntegration = () => {
  const { navigate } = useNavigation();
  const token = useAccessToken();
  const { analytics } = useAnalytics();
  const [integratedSystem, setIntegratedSystem] = React.useState({
    name: "",
    systemType: "",
  });
  const [error, setError] = React.useState();

  const [creating, setCreating] = React.useState(false);
  const handleCreate = () => {
    setCreating(true);
    createIntegratedSystemAsync({
      payload: integratedSystem,
      token,
    })
      .then((s) => {
        analytics.track("site:settings_integration_create_success");
        navigate(`/settings/integrations/detail/${s.id}`);
      })
      .catch((e) => {
        analytics.track("site:settings_integration_create_failure");
        setError(e);
      })
      .finally(() => setCreating(false));
  };
  return (
    <React.Fragment>
      <MoveUpHierarchyPrimaryButton to="/settings/integrations">
        Back to Integrations
      </MoveUpHierarchyPrimaryButton>
      <PageHeading title="Create New Integration" showHr />
      {error && <ErrorCard error={error} />}

      {!integratedSystem.systemType && (
        <div className="m-5">
          {systemTypes.map((t) => (
            <div
              onClick={() =>
                setIntegratedSystem({
                  ...integratedSystem,
                  systemType: t,
                })
              }
              key={t}
              className="p-3 mb-3 shadow bg-body rounded"
              style={{ cursor: "pointer" }}
            >
              <div className="row justify-content-center">
                <div className="col-3 text-center">
                  <h5>{t}</h5>
                </div>
                <div className="col-2">
                  <div style={{ maxWidth: "50px" }}>
                    <IntegrationIcon systemType={t} />
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {integratedSystem.systemType &&
        integratedSystem.systemType !== "Shopify" && (
          <GenericNextSteps
            handleCreate={handleCreate}
            integratedSystem={integratedSystem}
            setIntegratedSystem={setIntegratedSystem}
            creating={creating}
          />
        )}
      {integratedSystem.systemType &&
        integratedSystem.systemType === "Shopify" && <ShopifyNextSteps />}
    </React.Fragment>
  );
};
