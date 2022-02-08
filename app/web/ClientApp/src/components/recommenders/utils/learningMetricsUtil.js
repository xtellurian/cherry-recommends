import React from "react";
import AsyncSelectMetric from "../../molecules/selectors/AsyncSelectMetric";
import { BigPopup } from "../../molecules/popups/BigPopup";
import { EntityRow } from "../../molecules/layout/EntityRow";

import { EmptyList, ErrorCard, Spinner } from "../../molecules";

const LearningMetricRow = ({ metric, requestRemove }) => {
  if (metric.loading) {
    return <Spinner />;
  }
  return (
    <EntityRow>
      <div className="col">
        <h5>{metric.name}</h5>
      </div>
      <div className="col-md-3">
        <button
          onClick={() => requestRemove(metric)}
          className="btn btn-block btn-outline-danger"
        >
          Remove
        </button>
      </div>
    </EntityRow>
  );
};

export const LearningMetricsUtil = ({
  error,
  recommender,
  learningMetrics,
  setLearningMetrics,
  basePath,
}) => {
  const [isOpen, setIsOpen] = React.useState(false);
  const commonIds = learningMetrics?.map((_) => _.commonId) || [];
  const [newMetricOption, setNewMetricOption] = React.useState();
  const handleUpdateLearningMetrics = () => {
    commonIds.push(newMetricOption.value.commonId);
    setLearningMetrics(commonIds);
  };
  const handleRemove = (metric) => {
    console.log(metric);
    const newIds = commonIds.filter((_) => _ !== metric.commonId);
    console.log(newIds);
    setLearningMetrics(newIds);
  };
  return (
    <React.Fragment>
      <BigPopup isOpen={isOpen} setIsOpen={setIsOpen}>
        <h3>Add a Learning Metric</h3>
        {error && <ErrorCard error={error} />}
        <div style={{ minHeight: "200px" }}>
          <AsyncSelectMetric onChange={setNewMetricOption} />
        </div>
        <button
          className="btn btn-block btn-primary"
          onClick={handleUpdateLearningMetrics}
        >
          Add
        </button>
      </BigPopup>

      <h3>Learning Metrics</h3>
      {error && <ErrorCard error={error} />}

      {learningMetrics.map((f) => (
        <LearningMetricRow metric={f} key={f.id} requestRemove={handleRemove} />
      ))}
      {learningMetrics.length === 0 && (
        <EmptyList>There are no Learning Metrics yet.</EmptyList>
      )}

      <div className="mt-4">
        <button
          className="btn btn-primary float-right"
          onClick={() => setIsOpen(true)}
        >
          Add Learning Metric
        </button>
      </div>
    </React.Fragment>
  );
};
