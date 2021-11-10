import React from "react";
import { AsyncSelectFeature } from "../../molecules/selectors/AsyncSelectFeature";
import { BigPopup } from "../../molecules/popups/BigPopup";
import { EntityRow } from "../../molecules/layout/EntityRow";

import { EmptyList, ErrorCard, Spinner } from "../../molecules";

const LearningFeatureRow = ({ feature, requestRemove }) => {
  if (feature.loading) {
    return <Spinner />;
  }
  return (
    <EntityRow>
      <div className="col">
        <h5>{feature.name}</h5>
      </div>
      <div className="col-md-3">
        <button
          onClick={() => requestRemove(feature)}
          className="btn btn-block btn-outline-danger"
        >
          Remove
        </button>
      </div>
    </EntityRow>
  );
};

export const LearningFeaturesUtil = ({
  error,
  recommender,
  learningFeatures,
  setLearningFeatures,
  basePath,
}) => {
  const [isOpen, setIsOpen] = React.useState(false);
  const commonIds = learningFeatures?.map((_) => _.commonId) || [];
  const [newFeatureOption, setNewFeatureOption] = React.useState();
  const handleUpdateLearningFeatures = () => {
    commonIds.push(newFeatureOption.value.commonId);
    setLearningFeatures(commonIds);
  };
  const handleRemove = (feature) => {
    console.log(feature);
    const newIds = commonIds.filter((_) => _ !== feature.commonId);
    console.log(newIds);
    setLearningFeatures(newIds);
  };
  return (
    <React.Fragment>
      <BigPopup isOpen={isOpen} setIsOpen={setIsOpen}>
        <h3>Add a Learning Feature</h3>
        {error && <ErrorCard error={error} />}
        <div style={{ minHeight: "200px" }}>
          <AsyncSelectFeature onChange={setNewFeatureOption} />
        </div>
        <button
          className="btn btn-block btn-primary"
          onClick={handleUpdateLearningFeatures}
        >
          Add
        </button>
      </BigPopup>

      <h3>Learning Features</h3>
      {error && <ErrorCard error={error} />}

      {learningFeatures.map((f) => (
        <LearningFeatureRow
          feature={f}
          key={f.id}
          requestRemove={handleRemove}
        />
      ))}
      {learningFeatures.length === 0 && (
        <EmptyList>There are no Learning Features yet.</EmptyList>
      )}

      <div className="mt-4">
        <button className="btn btn-primary float-right" onClick={() => setIsOpen(true)}>
          Add Learning Feature
        </button>
      </div>
    </React.Fragment>
  );
};
