import React from "react";
import { useParams } from "react-router-dom";
import { Check } from "react-bootstrap-icons";
import { renameAsync } from "../../../api/integratedSystemsApi";
import { Top } from "./Top";
import { WebhookPanel } from "./WebhookPanel";
import { useIntegratedSystem } from "../../../api-hooks/integratedSystemsApi";
import { useAccessToken } from "../../../api-hooks/token";
import { Spinner, ErrorCard, Subtitle } from "../../molecules";
import { RenameEntityPopup } from "../../molecules/popups/RenameEntityPopup";
import { HubspotOverview } from "./hubspot/HubspotOverview";

export const IntegratedSystemDetail = () => {
  const token = useAccessToken();
  let { id } = useParams();
  const [trigger, setReloadTrigger] = React.useState({});
  const integratedSystem = useIntegratedSystem({ id, trigger });

  const [renameOpen, setRenameOpen] = React.useState(false);

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
      )}
      {!integratedSystem.loading && !integratedSystem.error && (
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
    </React.Fragment>
  );
};
