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
import { useFeatureFlag } from "../../launch-darkly/hooks";
import { useNavigation } from "../../../utility/useNavigation";

const systemTypes = ["Hubspot", "Segment", "Shopify", "Custom"];

export const CreateIntegration = () => {
  const { navigate } = useNavigation();
  const token = useAccessToken();
  const { analytics } = useAnalytics();
  const shopifyFlag = useFeatureFlag("shopify", true);
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
      <NoteBox className="m-3" label="What is an integration?">
        Integrations allow you to automatically pull data or push
        recommendations into various external systems. Select either Segment,
        Hubspot,{shopifyFlag ? " Shopify," : ""} or Custom below. Give your
        integration a name, for example: 'Production Segment Connection'
      </NoteBox>

      {!integratedSystem.systemType && (
        <div className="m-5">
          {systemTypes
            .filter((v) => {
              return !(!shopifyFlag && v.toLowerCase() === "shopify");
            })
            .map((t) => (
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

      {integratedSystem.systemType && (
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
            disabled={
              !integratedSystem.name || integratedSystem.name.length < 3
            }
            className="btn btn-primary btn-block mt-3"
            onClick={handleCreate}
          >
            Create
          </AsyncButton>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};
