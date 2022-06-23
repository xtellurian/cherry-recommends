import React from "react";
import { useParams } from "react-router";
import {
  usePromotionsCampaign,
  useAudience,
} from "../../../../api-hooks/promotionsCampaignsApi";
import {
  EmptyList,
  Spinner,
  PageHeading,
  MoveUpHierarchyPrimaryButton,
} from "../../../molecules";
import EntityDetailPageLayout from "../../../molecules/layout/EntityDetailPageLayout";
import { SegmentRow } from "../../../segments/SegmentRow";
import { AddSegmentPopup } from "../AddSegmentPopup";
import { RemoveSegmentPopup } from "../RemoveSegmentPopup";
import { useAccessToken } from "../../../../api-hooks/token";
import {
  addAudienceSegmentAsync,
  removeAudienceSegmentAsync,
} from "../../../../api/promotionsCampaignsApi";

export const ManageAudience = () => {
  const { id } = useParams();
  const recommender = usePromotionsCampaign({ id });
  const [trigger, setTrigger] = React.useState();
  const audience = useAudience({ id, trigger });
  const token = useAccessToken();

  const [segmentToRemove, setSegmentToRemove] = React.useState();
  const [isAddSegmentPopupOpen, setIsAddSegmentPopupOpen] =
    React.useState(false);
  const [isRemoveSegmentPopupOpen, setIsRemoveSegmentPopupOpen] =
    React.useState(false);

  const targetPlurals = {
    customer: " customers",
    business: " businesses",
  };

  const [addError, setAddError] = React.useState();
  const [adding, setAdding] = React.useState(false);
  const [removeError, setRemoveError] = React.useState();
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
    addAudienceSegmentAsync({
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
    removeAudienceSegmentAsync({
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
          title="Manage Audience"
          subtitle={recommender.name || "..."}
        />
      }
      options={
        <button
          onClick={() => setIsAddSegmentPopupOpen(true)}
          className="float-right btn btn-primary"
        >
          Add a Segment
        </button>
      }
    >
      <React.Fragment>
        {audience.loading && <Spinner>Loading Audience</Spinner>}
        {audience.segments &&
          audience.segments.map((i) => (
            <SegmentRow segment={i} key={i.id}>
              <div onClick={(e) => e.stopPropagation()}>
                <div className="text-right">
                  <button
                    className="btn btn-outline-primary"
                    onClick={() => onRemoveSegment(i)}
                  >
                    X
                  </button>
                </div>
              </div>
            </SegmentRow>
          ))}
        {(!audience.segments ||
          (audience.segments && audience.segments.length === 0)) && (
          <EmptyList>
            <span>
              This campaign works with all{" "}
              {targetPlurals[recommender.targetType]}.
            </span>
          </EmptyList>
        )}

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
      </React.Fragment>
    </EntityDetailPageLayout>
  );
};
