import React from "react";
import { useParams, useRouteMatch } from "react-router-dom";
import { Check } from "react-bootstrap-icons";
import {
  renameAsync,
  deleteIntegratedSystemAsync,
} from "../../../api/integratedSystemsApi";
import { Top } from "./Top";
import { WebhookPanel } from "./WebhookPanel";
import { useIntegratedSystem } from "../../../api-hooks/integratedSystemsApi";
import { useAccessToken } from "../../../api-hooks/token";
import { Spinner, ErrorCard, Subtitle } from "../../molecules";
import { RenameEntityPopup } from "../../molecules/popups/RenameEntityPopup";
import { ConfirmDeletePopup } from "../../molecules/popups/ConfirmDeletePopup";
import { HubspotOverview } from "./hubspot/HubspotOverview";
import { IntegrationIcon } from "./icons/IntegrationIcons";
import { ShopifyOverview } from "./shopify/ShopifyOverview";
import { useFeatureFlag } from "../../launch-darkly/hooks";
import { useNavigation } from "../../../utility/useNavigation";
import {
  useEnvironmentReducer,
  useEnvironments,
} from "../../../api-hooks/environmentsApi";
import { useQuery } from "../../../utility/utility";
import { LinkToDocs } from "./docs/LinkToDocs";

export const IntegratedSystemDetail = () => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const { path } = useRouteMatch();
  let { id } = useParams();
  const query = useQuery();
  const environmentId = query.get("environmentId");
  const environments = useEnvironments();
  const [currentEnvironment, setEnvironment] = useEnvironmentReducer();
  const [trigger, setReloadTrigger] = React.useState({});
  const integratedSystem = useIntegratedSystem({ id, trigger });
  const shopifyFlag = useFeatureFlag("shopify", true);

  const [renameOpen, setRenameOpen] = React.useState(false);
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [deleteError, setDeleteError] = React.useState();

  const handleDelete = () => {
    deleteIntegratedSystemAsync({ id, token })
      .then(setDeleteOpen(false))
      .then(() => navigate("/settings/integrations"))
      .catch(setDeleteError);
  };

  React.useEffect(() => {
    if (
      environmentId &&
      !isNaN(environmentId) &&
      !environments.loading &&
      environments.items &&
      (!currentEnvironment ||
        (currentEnvironment && currentEnvironment.id !== ~~environmentId))
    ) {
      const environment = environments.items.find(
        (e) => e.id === ~~environmentId
      );
      if (environment) {
        setEnvironment(environment);
      } else {
        console.warn("Environment not found!");
      }
    }
  }, [environments, currentEnvironment, environmentId]);

  React.useEffect(() => {
    // Remove environmentId from the url
    if (currentEnvironment && path && environmentId) {
      console.debug("Navigation to", path);
      navigate({
        pathnmame: path,
        search: null,
      });
    }
  }, [currentEnvironment, path, environmentId]);

  return (
    <React.Fragment>
      <Top integratedSystem={integratedSystem} />
      <hr />
      {integratedSystem.loading && (
        <Spinner>Loading System Information</Spinner>
      )}
      {integratedSystem.integrationStatus === "ok" && (
        <div className="text-success float-right">
          <Check size={42} className="mr-1" />
          Connected
        </div>
      )}
      {integratedSystem.name && (
        <React.Fragment>
          <RenameEntityPopup
            onRename={(name) =>
              renameAsync({ token, id: integratedSystem.id, name })
                .then(setReloadTrigger)
                .catch(() => alert("Rename error"))
            }
            isOpen={renameOpen}
            setIsOpen={setRenameOpen}
            entity={integratedSystem}
            label="Rename Integrated System"
          />
          <ConfirmDeletePopup
            handleDelete={handleDelete}
            error={deleteError}
            open={deleteOpen}
            setOpen={setDeleteOpen}
            entity={integratedSystem}
            label="Delete Integrated System"
          />
        </React.Fragment>
      )}
      {!integratedSystem.loading && !integratedSystem.error && (
        <div className="d-flex justify-content-around">
          <div>
            <Subtitle>Information</Subtitle>
            <div>
              Name: {integratedSystem.name}{" "}
              <button
                className="btn btn-link btn-sm"
                onClick={() => setRenameOpen(true)}
              >
                (rename)
              </button>
            </div>
            <div>Type: {integratedSystem.systemType}</div>
            <div>Status: {integratedSystem.integrationStatus}</div>
            <div className="m-2">
              <button
                className="btn btn-danger"
                onClick={() => setDeleteOpen(true)}
              >
                Delete
              </button>
            </div>
          </div>
          <div className="d-flex flex-column">
            <IntegrationIcon integration={integratedSystem} />
            <div>
              <LinkToDocs>Learn More</LinkToDocs>
            </div>
          </div>
        </div>
      )}

      <hr />
      {!integratedSystem.loading && !integratedSystem.error && (
        <div className="row">
          {integratedSystem &&
            integratedSystem.id &&
            integratedSystem.systemType === "segment" && (
              <div className="col">
                <WebhookPanel integratedSystemId={integratedSystem.id} />
              </div>
            )}
        </div>
      )}

      {integratedSystem.error && <ErrorCard error={integratedSystem.error} />}
      {integratedSystem.systemType === "hubspot" && (
        <HubspotOverview integratedSystem={integratedSystem} />
      )}
      {shopifyFlag && integratedSystem.systemType === "shopify" && (
        <ShopifyOverview integratedSystem={integratedSystem} />
      )}
    </React.Fragment>
  );
};
