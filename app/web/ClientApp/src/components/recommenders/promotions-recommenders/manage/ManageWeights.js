import React from "react";

import { RecommenderCard } from "../../RecommenderCard";
import {
  ManualControlSetAll,
  PromotionOptimiserWeightControl,
} from "./PromotionOptimiserWeightControl";
import {
  useOptimiser,
  usePromotionsRecommender,
} from "../../../../api-hooks/promotionsRecommendersApi";
import { useParams } from "react-router-dom";
import { BigPopup } from "../../../molecules/popups/BigPopup";
import {
  ErrorCard,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
} from "../../../molecules";
import { Spinner } from "reactstrap";
import { ManageNav } from "./Tabs";
import { UseOptimiserControl } from "../UseOptimiserControl";

const WeightsCard = ({ recommender, trigger }) => {
  return (
    <RecommenderCard title="Weights">
      {recommender.error && <ErrorCard error={recommender.error} />}
      {recommender.loading && <Spinner />}
      {recommender.id ? (
        <React.Fragment>
          <PromotionOptimiserWeightControl
            recommender={recommender}
            trigger={trigger}
          />
        </React.Fragment>
      ) : null}
    </RecommenderCard>
  );
};

const Weights = () => {
  const { id } = useParams();
  const [trigger, setTrigger] = React.useState({});
  const recommender = usePromotionsRecommender({ id, trigger });
  const optimiser = useOptimiser({ id: recommender.useOptimiser ? id : null });

  const [isManualControlPopupOpen, setIsManualControlPopupOpen] =
    React.useState(false);

  const onManualSaved = () => {
    setTrigger({});
    setIsManualControlPopupOpen(false);
  };

  return (
    <React.Fragment>
      <button
        disabled={!recommender.useOptimiser}
        className="btn btn-primary float-right"
        onClick={() => setIsManualControlPopupOpen(true)}
      >
        Edit All
      </button>

      <BigPopup
        isOpen={isManualControlPopupOpen}
        setIsOpen={setIsManualControlPopupOpen}
      >
        {optimiser && recommender && recommender.items ? (
          <ManualControlSetAll
            optimiser={optimiser}
            promotions={recommender.items}
            onSaved={onManualSaved}
          />
        ) : (
          <Spinner />
        )}
      </BigPopup>
      <MoveUpHierarchyPrimaryButton
        to={{
          pathname: `/recommenders/promotions-recommenders/detail/${id}`,
          search: null,
        }}
      >
        Back to Recommender
      </MoveUpHierarchyPrimaryButton>
      <PageHeading
        title="Manage Weights"
        subtitle={recommender.name || "..."}
      />
      <ManageNav id={id} />
      <div className="d-flex flex-row-reverse">
        <UseOptimiserControl recommender={recommender} onUpdated={setTrigger} />
        <span className="p-3 text-center">Use Optimiser</span>
      </div>
      <WeightsCard recommender={recommender} trigger={trigger} />
    </React.Fragment>
  );
};

export default Weights;
