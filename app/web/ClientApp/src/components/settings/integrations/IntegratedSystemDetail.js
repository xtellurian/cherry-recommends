import React from "react";
import { useParams } from "react-router-dom";
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

export const IntegratedSystemDetail = () => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
  let { id } = useParams();
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
        <div className="row justify-content-center">
          <div className="col-4">
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
          <div className="col-2">
            <IntegrationIcon integration={integratedSystem} />
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
