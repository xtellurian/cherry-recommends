import React from "react";
import { useParams } from "react-router-dom";
import {
  useParameterSetCampaign,
  useReportImageBlobUrl,
} from "../../../api-hooks/parameterSetCampaignsApi";
import {
  deleteParameterSetCampaignAsync,
  createParameterSetCampaignAsync,
} from "../../../api/parameterSetCampaignsApi";
import { ErrorCard, Spinner, ExpandableCard } from "../../molecules";
import { ConfirmationPopup } from "../../molecules/popups/ConfirmationPopup";
import { JsonView } from "../../molecules/JsonView";

import { CopyableField } from "../../molecules/fields/CopyableField";
import { useAccessToken } from "../../../api-hooks/token";
import { CloneCampaign } from "../utils/CloneCampaign";
import { ParameterRow } from "../../parameters/ParameterRow";
import { ParameterSetCampaignLayout } from "./ParameterSetCampaignLayout";
import { ViewReportImagePopup } from "../utils/ViewImagePopup";
import { useNavigation } from "../../../utility/useNavigation";
import { useTenantName } from "../../tenants/PathTenantProvider";

const tabs = [
  { id: "detail", label: "Detail" },
  { id: "arguments", label: "Arguments" },
  { id: "metrics", label: "Learning Metrics" },
];
export const ParameterSetCampaignDetail = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const [reportOpen, setReportOpen] = React.useState(false);
  const [trigger, setTrigger] = React.useState();
  const recommender = useParameterSetCampaign({ id, trigger });
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [cloneOpen, setCloneOpen] = React.useState(false);
  const [deleteError, setDeleteError] = React.useState();
  const onDeleted = () => {
    navigate("/campaigns/parameter-set-campaigns");
  };

  const cloneAsync = (name, commonId) => {
    return createParameterSetCampaignAsync({
      token,
      payload: {
        name,
        commonId,
        cloneFromId: recommender.id,
      },
    });
  };

  const { tenantName } = useTenantName();
  const tenantParam = tenantName !== "" ? `?x-tenant=${tenantName}` : "";

  return (
    <React.Fragment>
      <ConfirmationPopup
        isOpen={deleteOpen}
        setIsOpen={setDeleteOpen}
        label="Are you sure you want to delete this model?"
      >
        <div className="m-2">{recommender.name}</div>
        {deleteError && <ErrorCard error={deleteError} />}
        <div
          className="btn-group"
          role="group"
          aria-label="Delete or cancel buttons"
        >
          <button
            className="btn btn-secondary"
            onClick={() => setDeleteOpen(false)}
          >
            Cancel
          </button>
          <button
            className="btn btn-danger"
            onClick={() => {
              deleteParameterSetCampaignAsync({
                token,
                id: recommender.id,
              })
                .then(() => {
                  setDeleteOpen(false);
                  if (onDeleted) {
                    onDeleted();
                  }
                })
                .catch(setDeleteError);
            }}
          >
            Delete
          </button>
        </div>
      </ConfirmationPopup>
      <ParameterSetCampaignLayout>
        {recommender.loading && <Spinner>Loading Campaign</Spinner>}
        {recommender.error && <ErrorCard error={recommender.error} />}

        <div className="row mb-2">
          <div className="col">
            {recommender.commonId && (
              <CopyableField label="Common Id" value={recommender.commonId} />
            )}

            {recommender.id && (
              <CopyableField
                label="Invokation URL"
                value={`${window.location.protocol}//${window.location.host}/api/Campaigns/ParameterSetCampaigns/${recommender.id}/invoke${tenantParam}`}
              />
            )}

            {recommender.parameters && (
              <React.Fragment>
                <div>Parameters</div>
                {recommender.parameters.map((p) => (
                  <ParameterRow key={p.id} parameter={p} disableDelete={true} />
                ))}
              </React.Fragment>
            )}
          </div>
        </div>

        <div className="d-flex flex-row-reverse">
          {!recommender.loading && !recommender.error && (
            <React.Fragment>
              <button
                className="btn btn-primary mr-1"
                onClick={() => setReportOpen(true)}
              >
                Show Latest Report
              </button>
              <button
                className="btn btn-outline-primary mr-1"
                onClick={() => setCloneOpen(true)}
              >
                Clone this Campaign
              </button>
              <button
                className="btn btn-link"
                onClick={() => setDeleteOpen(true)}
              >
                Delete
              </button>
              <ConfirmationPopup
                isOpen={cloneOpen}
                setIsOpen={setCloneOpen}
                label="Clone this campaign?"
              >
                <CloneCampaign
                  recommender={recommender}
                  cloneAsync={cloneAsync}
                  onCloned={(r) =>
                    navigate(
                      `/campaigns/parameter-set-campaigns/detail/${r.id}`
                    )
                  }
                />
              </ConfirmationPopup>

              <ViewReportImagePopup
                isOpen={reportOpen}
                setIsOpen={setReportOpen}
                id={id}
                useReportImageBlobUrl={useReportImageBlobUrl}
              />
            </React.Fragment>
          )}
        </div>

        <div className="mt-3">
          {!recommender.loading && (
            <ExpandableCard label="More Detail">
              <JsonView data={recommender} />
            </ExpandableCard>
          )}
        </div>
      </ParameterSetCampaignLayout>
    </React.Fragment>
  );
};
