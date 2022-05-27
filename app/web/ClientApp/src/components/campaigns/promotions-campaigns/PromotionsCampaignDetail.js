import React from "react";
import { useParams } from "react-router-dom";

import {
  useAudience,
  usePromotionsCampaign,
} from "../../../api-hooks/promotionsCampaignsApi";
import {
  deletePromotionsCampaignAsync,
  createPromotionsCampaignAsync,
} from "../../../api/promotionsCampaignsApi";
import { PromotionRow } from "../../promotions/PromotionRow";
import { useAccessToken } from "../../../api-hooks/token";
import {
  Subtitle,
  Spinner,
  ErrorCard,
  EmptyList,
  Navigation,
} from "../../molecules";
import { ConfirmationPopup } from "../../molecules/popups/ConfirmationPopup";
import { CopyableField } from "../../molecules/fields/CopyableField";
import { EntityField } from "../../molecules/EntityField";
import { CloneCampaign } from "../utils/CloneCampaign";
import { GettingStartedSection } from "./GettingStartedSection";
import { PromotionCampaignLayout } from "./PromotionCampaignLayout";

import { SegmentRow } from "../../segments/SegmentRow";
import { useFeatureFlag } from "../../launch-darkly/hooks";
import { useNavigation } from "../../../utility/useNavigation";
import { useTenantName } from "../../tenants/PathTenantProvider";

export const PromotionCampaignClone = ({ iconClassName }) => {
  const { id } = useParams();
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const [trigger, setTrigger] = React.useState();
  const recommender = usePromotionsCampaign({ id, trigger });
  const [cloneOpen, setCloneOpen] = React.useState(false);

  const cloneAsync = (name, commonId) => {
    return createPromotionsCampaignAsync({
      token,
      payload: {
        name,
        commonId,
        cloneFromId: recommender.id,
        itemIds: recommender.items?.map((o) => `${o.id}`),
      },
    });
  };

  return (
    <React.Fragment>
      <span
        className={`cursor-pointer ${iconClassName}`}
        onClick={() => setCloneOpen(true)}
      >
        <img
          src="/icons/clone.svg"
          role="img"
          alt="Clone Icon"
          className="mr-2"
        />
        Clone
      </span>
      <ConfirmationPopup
        isOpen={cloneOpen}
        setIsOpen={setCloneOpen}
        label="Clone this campaign?"
      >
        <CloneCampaign
          recommender={recommender}
          cloneAsync={cloneAsync}
          onCloned={(r) =>
            navigate(`/campaigns/promotions-campaigns/detail/${r.id}`)
          }
        />
      </ConfirmationPopup>
    </React.Fragment>
  );
};

export const PromotionCampaignDelete = ({ iconClassName }) => {
  const { id } = useParams();
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const [trigger, setTrigger] = React.useState();
  const recommender = usePromotionsCampaign({ id, trigger });
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [deleteError, setDeleteError] = React.useState();

  const onDeleted = () => {
    navigate("/campaigns/promotions-campaigns");
  };

  return (
    <React.Fragment>
      <span
        className={`cursor-pointer ${iconClassName}`}
        onClick={() => setDeleteOpen(true)}
      >
        <img
          src="/icons/delete.svg"
          role="img"
          alt="Delete Icon"
          className="mr-2"
        />
        Delete
      </span>

      <ConfirmationPopup
        isOpen={deleteOpen}
        setIsOpen={setDeleteOpen}
        label="Are you sure you want to delete this Campaign?"
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
              deletePromotionsCampaignAsync({
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
    </React.Fragment>
  );
};

export const CampaignDetail = () => {
  return (
    <PromotionCampaignLayout>
      <CampaignDetailSection />
    </PromotionCampaignLayout>
  );
};

const CampaignDetailSection = () => {
  const { id } = useParams();

  const [trigger, setTrigger] = React.useState();
  const recommender = usePromotionsCampaign({ id, trigger });
  const audience = useAudience({ id, trigger });
  const segmentFlag = useFeatureFlag("segments", true);
  const { tenantName } = useTenantName();
  const tenantParam = tenantName !== "" ? `?x-tenant=${tenantName}` : "";

  const targetPlurals = {
    customer: "customers",
    business: "businesses",
  };

  return (
    <React.Fragment>
      {recommender.loading && <Spinner>Loading Campaign</Spinner>}
      {recommender.error && <ErrorCard error={recommender.error} />}

      {!recommender.loading && (
        <div className="row">
          <div className="col">
            <GettingStartedSection recommender={recommender} />
          </div>
        </div>
      )}

      <div className="row">
        <div className="col">
          {recommender.commonId && (
            <CopyableField label="Common Id" value={recommender.commonId} />
          )}
          {recommender.targetType && (
            <CopyableField label="Target" value={recommender.targetType} />
          )}

          {recommender.id && (
            <CopyableField
              label="Invokation URL"
              value={`${window.location.protocol}//${window.location.host}/api/Campaigns/PromotionsCampaigns/${recommender.id}/invoke${tenantParam}`}
            />
          )}

          {recommender.baselineItem && (
            <EntityField
              label="Baseline Promotion"
              entity={recommender.baselineItem}
              to={`/promotions/promotions/detail/${recommender.baselineItemId}`}
            />
          )}

          {recommender.targetMetric && (
            <EntityField
              label="Target Metric"
              entity={recommender.targetMetric}
              to={`/metrics/metrics/detail/${recommender.targetMetric.id}`}
            />
          )}
        </div>
      </div>

      <hr />

      <div className="row mb-3">
        <div className="col">
          <div className="mb-4">
            <Navigation
              to={{
                pathname: `/campaigns/promotions-campaigns/manage/promotions/${id}`,
                search: null,
              }}
            >
              <button className="float-right btn btn-outline-primary">
                Manage Promotions
              </button>
            </Navigation>
            <Subtitle>Associated Promotions</Subtitle>
          </div>
          {recommender.items &&
            recommender.items.map((i) => (
              <PromotionRow promotion={i} key={i.id} />
            ))}
          {recommender.items && recommender.items.length === 0 && (
            <EmptyList>This campaign works with all promotions.</EmptyList>
          )}
        </div>
        {segmentFlag && (
          <div className="col">
            <div className="mb-4">
              <Subtitle>Associated Audience</Subtitle>
            </div>
            {audience.segments &&
              audience.segments.map((i) => (
                <SegmentRow segment={i} key={i.id} />
              ))}
            {(!audience.segments ||
              (audience.segments && audience.segments.length === 0)) && (
              <EmptyList>
                This campaign works with all{" "}
                {targetPlurals[recommender.targetType]}.
              </EmptyList>
            )}
          </div>
        )}
      </div>
    </React.Fragment>
  );
};
