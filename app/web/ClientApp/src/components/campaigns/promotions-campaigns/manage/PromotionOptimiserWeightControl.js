import React from "react";
import { useAccessToken } from "../../../../api-hooks/token";
import {
  fetchPromotionOptimiserAsync,
  setPromotionOptimiserWeightAsync,
  setAllPromotionOptimiserWeightsAsync,
} from "../../../../api/promotionsCampaignsApi";
import {
  AsyncButton,
  EmptyState,
  ErrorCard,
  Spinner,
} from "../../../molecules";
import EntityFlexRow from "../../../molecules/layout/EntityFlexRow";
import { InputGroup, TextInput } from "../../../molecules/TextInput";

import "./weights.css";

const range = {
  min: 0,
  max: 1.5,
  step: 0.05,
};

const useUpdatableOptimiser = ({ id, useOptimiser, trigger }) => {
  const [optimiser, setOptimiser] = React.useState();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(true);
  const token = useAccessToken();
  React.useEffect(() => {
    if (token && useOptimiser) {
      setError();
      fetchPromotionOptimiserAsync({ id, token })
        .then(setOptimiser)
        .catch(setError(setError))
        .finally(() => setLoading(false));
    }
  }, [token, trigger]);

  const update = ({ weightId, weight }) => {
    if (token && id && weightId && useOptimiser) {
      setError();
      setLoading(true);
      setPromotionOptimiserWeightAsync({ token, id, weightId, weight })
        .then(setOptimiser)
        .catch(setError)
        .finally(() => setLoading(false));
    }
  };

  return { optimiser, loading, error, update };
};

const ManualControlRow = ({
  id,
  weight,
  segmentId,
  promotionId,
  setWeight,
  promotions,
}) => {
  const promotion = promotions.find((_) => _.id === promotionId);
  return (
    <React.Fragment>
      <InputGroup>
        <TextInput
          label={promotion.name}
          value={weight}
          onChange={(e) => setWeight(e.target.value)}
          type="number"
          min={range.min}
          max={range.max}
          step={range.step}
        />
      </InputGroup>
    </React.Fragment>
  );
};
// this one lets you enter all teh values at once, without renormalising in between
export const ManualControlSetAll = ({ optimiser, promotions, onSaved }) => {
  const [weights, setWeights] = React.useState(optimiser.weights);
  const handleSetWeight = (weightId, weight) => {
    weights.find((_) => _.id === weightId).weight = weight;
    setWeights([...weights]);
  };
  const token = useAccessToken();
  const [saving, setSaving] = React.useState(false);
  const [error, setError] = React.useState();

  const handleSave = () => {
    setError();
    setSaving(true);
    setAllPromotionOptimiserWeightsAsync({
      token,
      id: optimiser.recommenderId,
      weights,
    })
      .then(onSaved)
      .catch(setError)
      .finally(() => setSaving(false));
  };
  return (
    <React.Fragment>
      {error ? <ErrorCard error={error} /> : null}
      <div>
        {weights
          ? weights.map((w) => (
              <ManualControlRow
                key={w.id}
                {...w}
                promotions={promotions}
                setWeight={(v) => handleSetWeight(w.id, v)}
              />
            ))
          : null}
      </div>
      <AsyncButton
        loading={saving}
        onClick={handleSave}
        className="btn btn-primary btn-block mt-3"
      >
        Save
      </AsyncButton>
    </React.Fragment>
  );
};

// this is the standard weight control that auto-updates 1 weight at a time
// good - auto-renormalise
// bad - hard to enter specifc values
const WeightControl = ({
  id,
  loading,
  weight,
  segmentId,
  promotionId,
  update,
  promotions,
}) => {
  const promotion = promotions.find((_) => _.id === promotionId);
  const [internalValue, setInternalValue] = React.useState(weight);
  React.useEffect(() => {
    setInternalValue(weight);
  }, [weight]);

  const handleOnChange = (v) => {
    setInternalValue(v.target.value);
  };
  const handleUpdate = () => {
    if (weight !== internalValue) {
      update({ weightId: id, weight: internalValue });
    }
  };

  return (
    <React.Fragment>
      <EntityFlexRow>
        <span className="flex-grow-1 pr-2">
          {promotion.name}
          <input
            disabled={loading}
            type="range"
            min={range.min}
            max={range.max}
            step={range.step}
            value={internalValue}
            onChange={handleOnChange}
            onMouseUp={handleUpdate}
            className="cherry-range-picker form-range w-100"
            id="customRange1"
          />
        </span>
        <span className="flex-shrink-1">
          <TextInput
            onChange={handleOnChange}
            onBlur={handleUpdate}
            disabled={loading}
            type="number"
            value={internalValue}
            min={range.min}
            max={range.max}
            step={range.step}
          />
        </span>
      </EntityFlexRow>
    </React.Fragment>
  );
};

export const PromotionOptimiserWeightControl = ({ recommender, trigger }) => {
  const { optimiser, loading, error, update } = useUpdatableOptimiser({
    id: recommender.id,
    useOptimiser: recommender.useOptimiser,
    trigger,
  });

  if (recommender.useOptimiser) {
    return (
      <React.Fragment>
        {loading ? <Spinner /> : null}
        {error ? <ErrorCard error={error} /> : null}
        {optimiser && optimiser.weights
          ? optimiser.weights.map((w) => (
              <WeightControl
                key={w.id}
                {...w}
                promotions={recommender.items}
                update={update}
              />
            ))
          : null}
      </React.Fragment>
    );
  } else {
    return <EmptyState>Optimiser is not enabled.</EmptyState>;
  }
};
