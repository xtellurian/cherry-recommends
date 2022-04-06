import React from "react";
import { useParams } from "react-router-dom";

import {
  useAudience,
  usePromotionsRecommender,
} from "../../../api-hooks/promotionsRecommendersApi";
import {
  deletePromotionsRecommenderAsync,
  createPromotionsRecommenderAsync,
} from "../../../api/promotionsRecommendersApi";
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
import { CloneRecommender } from "../utils/CloneRecommender";
import { GettingStartedSection } from "./GettingStartedSection";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";

import { SegmentRow } from "../../segments/SegmentRow";
import { useFeatureFlag } from "../../launch-darkly/hooks";
import { useNavigation } from "../../../utility/useNavigation";

export const ItemRecommenderClone = ({ iconClassName }) => {
  const { id } = useParams();
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const [trigger, setTrigger] = React.useState();
  const recommender = usePromotionsRecommender({ id, trigger });
  const [cloneOpen, setCloneOpen] = React.useState(false);

  const cloneAsync = (name, commonId) => {
    return createPromotionsRecommenderAsync({
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
        label="Clone this recommender?"
      >
        <CloneRecommender
          recommender={recommender}
          cloneAsync={cloneAsync}
          onCloned={(r) =>
            navigate(`/recommenders/promotions-recommenders/detail/${r.id}`)
          }
        />
      </ConfirmationPopup>
    </React.Fragment>
  );
};

export const ItemRecommenderDelete = ({ iconClassName }) => {
  const { id } = useParams();
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const [trigger, setTrigger] = React.useState();
  const recommender = usePromotionsRecommender({ id, trigger });
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [deleteError, setDeleteError] = React.useState();

  const onDeleted = () => {
    navigate("/recommenders/promotions-recommenders");
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
        label="Are you sure you want to delete this Recommender?"
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
              deletePromotionsRecommenderAsync({
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

export const RecommenderDetail = () => {
  return (
    <ItemRecommenderLayout>
      <RecommenderDetailSection />
    </ItemRecommenderLayout>
  );
};

const RecommenderDetailSection = () => {
  const { id } = useParams();

  const [trigger, setTrigger] = React.useState();
  const recommender = usePromotionsRecommender({ id, trigger });
  const audience = useAudience({ id, trigger });
  const segmentFlag = useFeatureFlag("segments", true);

  const targetPlurals = {
    customer: "customers",
    business: "businesses",
  };

  return (
    <React.Fragment>
      {recommender.loading && <Spinner>Loading Recommender</Spinner>}
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
              value={`${window.location.protocol}//${window.location.host}/api/Recommenders/ItemsRecommenders/${recommender.id}/invoke`}
            />
          )}

          {recommender.baselineItem && (
            <EntityField
              label="Baseline Promotion"
              entity={recommender.baselineItem}
              to={`/promotions/detail/${recommender.baselineItemId}`}
            />
          )}

          {recommender.targetMetric && (
            <EntityField
              label="Target Metric"
              entity={recommender.targetMetric}
              to={`/metrics/detail/${recommender.targetMetric.id}`}
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
                pathname: `/recommenders/promotions-recommenders/manage/promotions/${id}`,
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
            <EmptyList>This recommender works with all promotions.</EmptyList>
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
                This recommender works with all{" "}
                {targetPlurals[recommender.targetType]}.
              </EmptyList>
            )}
          </div>
        )}
      </div>
    </React.Fragment>
  );
};
