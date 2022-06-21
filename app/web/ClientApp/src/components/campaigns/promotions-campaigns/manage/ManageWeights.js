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
import { AddOptimiserSegmentPopup } from "../AddOptimiserSegmentPopup";
import { RemoveOptimiserSegmentPopup } from "../RemoveOptimiserSegmentPopup";
import EntityDetailPageLayout from "../../../molecules/layout/EntityDetailPageLayout";

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

  const [isAddDistributionPopupOpen, setIsAddDistributionPopupOpen] =
    React.useState(false);
  const [isRemoveDistributionPopupOpen, setIsRemoveDistributionPopupOpen] =
    React.useState(false);
  const [segmentToRemove, setSegmentToRemove] = React.useState();

  const handleRemove = (segment) => {
    setSegmentToRemove(segment);
    setIsRemoveDistributionPopupOpen(true);
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
          onRemoveClicked={() => handleRemove(segment)}
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
          onClick={() => setIsAddDistributionPopupOpen(true)}
          className="float-right btn btn-primary"
        >
          Add Segment
        </button>
      }
    >
      <React.Fragment>
        <AddOptimiserSegmentPopup
          isOpen={isAddDistributionPopupOpen}
          setIsOpen={setIsAddDistributionPopupOpen}
          recommender={recommender}
          onAdded={setTrigger}
        />
        <RemoveOptimiserSegmentPopup
          isOpen={isRemoveDistributionPopupOpen}
          setIsOpen={setIsRemoveDistributionPopupOpen}
          recommender={recommender}
          onRemoved={setTrigger}
          segment={segmentToRemove}
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
