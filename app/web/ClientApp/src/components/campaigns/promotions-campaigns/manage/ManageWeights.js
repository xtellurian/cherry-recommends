import React, { useMemo } from "react";

import { CampaignCard } from "../../CampaignCard";
import {
  ManualControlSetAll,
  PromotionOptimiserWeightControl,
} from "./PromotionOptimiserWeightControl";
import {
  useOptimiser,
  useOptimiserSegments,
  usePromotionsCampaign,
} from "../../../../api-hooks/promotionsCampaignsApi";
import { useParams } from "react-router-dom";
import { BigPopup } from "../../../molecules/popups/BigPopup";
import {
  ErrorCard,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
  Accordion,
} from "../../../molecules";
import { Spinner } from "reactstrap";
import { ManageNav } from "./Tabs";
import { UseOptimiserControl } from "../UseOptimiserControl";
import { AddSegmentPopup } from "../AddSegmentPopup";
import { RemoveSegmentPopup } from "../RemoveSegmentPopup";
import EntityDetailPageLayout from "../../../molecules/layout/EntityDetailPageLayout";
import { useAccessToken } from "../../../../api-hooks/token";
import {
  addPromotionOptimiserSegmentAsync,
  removePromotionOptimiserSegmentAsync,
} from "../../../../api/promotionsCampaignsApi";

const DistributionRow = ({ recommender, segment, onRemoveClicked }) => {
  const [trigger, setTrigger] = React.useState({});
  const [isManualControlPopupOpen, setIsManualControlPopupOpen] =
    React.useState(false);

  const onManualSaved = () => {
    setTrigger({});
    setIsManualControlPopupOpen(false);
  };

  return (
    <div className="mb-2">
      <div className="text-right">
        <button
          disabled={!recommender.useOptimiser}
          className="btn btn-primary mr-1"
          onClick={() => setIsManualControlPopupOpen(true)}
        >
          Edit All
        </button>
        {segment?.id && (
          <button
            className="btn btn-outline-danger mr-1"
            onClick={onRemoveClicked}
          >
            Remove
          </button>
        )}
      </div>
      <WeightsCard
        recommender={recommender}
        segmentId={segment?.id}
        trigger={trigger}
        setTrigger={setTrigger}
      />
      <BigPopup
        isOpen={isManualControlPopupOpen}
        setIsOpen={setIsManualControlPopupOpen}
        header="Edit Weights"
        headerDivider
      >
        {recommender && recommender.items ? (
          <ManualControlSetAll
            recommender={recommender}
            segmentId={segment?.id}
            promotions={recommender.items}
            onSaved={onManualSaved}
          />
        ) : (
          <Spinner />
        )}
      </BigPopup>
    </div>
  );
};

const WeightsCard = ({ recommender, segmentId, trigger, setTrigger }) => {
  return (
    <CampaignCard title="Weights">
      {recommender.error && <ErrorCard error={recommender.error} />}
      {recommender.loading && <Spinner />}
      {recommender.id ? (
        <React.Fragment>
          <PromotionOptimiserWeightControl
            recommender={recommender}
            segmentId={segmentId}
            trigger={trigger}
            setTrigger={setTrigger}
          />
        </React.Fragment>
      ) : null}
    </CampaignCard>
  );
};

const Weights = () => {
  const { id } = useParams();
  const [trigger, setTrigger] = React.useState({});
  const recommender = usePromotionsCampaign({ id, trigger });
  const optimiser = useOptimiser({ id: recommender.useOptimiser ? id : null });
  const optimiserSegments = useOptimiserSegments({ id, optimiser, trigger });
  const token = useAccessToken();

  const [isAddSegmentPopupOpen, setIsAddSegmentPopupOpen] =
    React.useState(false);
  const [isRemoveSegmentPopupOpen, setIsRemoveSegmentPopupOpen] =
    React.useState(false);
  const [segmentToRemove, setSegmentToRemove] = React.useState();
  const [addError, setAddError] = React.useState();
  const [removeError, setRemoveError] = React.useState();
  const [adding, setAdding] = React.useState(false);
  const [removing, setRemoving] = React.useState(false);

  React.useEffect(() => {
    if (!isAddSegmentPopupOpen) {
      setAddError(null);
    }
  }, [isAddSegmentPopupOpen]);

  const onRemoveSegment = (segment) => {
    setSegmentToRemove(segment);
    setIsRemoveSegmentPopupOpen(true);
  };

  const handleAdd = (segment) => {
    setAddError(null);
    setAdding(true);
    addPromotionOptimiserSegmentAsync({
      token,
      id: recommender.id,
      segmentId: segment.id,
    })
      .then((r) => {
        setTrigger(r);
        setIsAddSegmentPopupOpen(false);
      })
      .catch(setAddError)
      .finally(() => setAdding(false));
  };

  const handleRemove = (segment) => {
    setRemoveError(null);
    setRemoving(true);
    removePromotionOptimiserSegmentAsync({
      token,
      id: recommender.id,
      segmentId: segment.id,
    })
      .then((r) => {
        setTrigger(r);
        setIsRemoveSegmentPopupOpen(false);
      })
      .catch(setRemoveError)
      .finally(() => setRemoving(false));
  };

  const distributionPanels = useMemo(() => {
    const items = optimiserSegments?.items || [];

    const distributionPanels = items.map((segment) => ({
      label: segment.name,
      content: (
        <DistributionRow
          key={segment.id}
          recommender={recommender}
          segment={segment}
          onRemoveClicked={() => onRemoveSegment(segment)}
        />
      ),
    }));

    return [
      {
        label: "Default",
        content: <DistributionRow recommender={recommender}></DistributionRow>,
      },
      ...distributionPanels,
    ];
  }, [optimiserSegments.items, recommender]);

  return (
    <EntityDetailPageLayout
      backButton={
        <MoveUpHierarchyPrimaryButton
          to={{
            pathname: `/campaigns/promotions-campaigns/detail/${id}`,
            search: null,
          }}
        >
          Back to Campaign
        </MoveUpHierarchyPrimaryButton>
      }
      header={
        <PageHeading
          title="Manage Weights"
          subtitle={recommender.name || "..."}
        />
      }
      options={
        <button
          disabled={!recommender.useOptimiser}
          onClick={() => setIsAddSegmentPopupOpen(true)}
          className="float-right btn btn-primary"
        >
          Add Segment
        </button>
      }
    >
      <React.Fragment>
        <AddSegmentPopup
          isOpen={isAddSegmentPopupOpen}
          setIsOpen={setIsAddSegmentPopupOpen}
          onAdd={handleAdd}
          error={addError}
          loading={adding}
        />
        <RemoveSegmentPopup
          isOpen={isRemoveSegmentPopupOpen}
          setIsOpen={setIsRemoveSegmentPopupOpen}
          recommender={recommender}
          onRemove={handleRemove}
          segment={segmentToRemove}
          error={removeError}
          loading={removing}
        />
        <ManageNav id={id} />
        <div className="d-flex flex-row-reverse">
          <UseOptimiserControl
            recommender={recommender}
            onUpdated={setTrigger}
          />
          <span className="p-3 text-center">Use Optimiser</span>
        </div>

        <Accordion panels={distributionPanels} />
      </React.Fragment>
    </EntityDetailPageLayout>
  );
};

export default Weights;
