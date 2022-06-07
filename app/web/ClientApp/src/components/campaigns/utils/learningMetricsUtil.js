import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";

import AsyncSelectMetric from "../../molecules/selectors/AsyncSelectMetric";
import { BigPopup } from "../../molecules/popups/BigPopup";
import { ErrorCard, Spinner, Typography } from "../../molecules";

const LearningMetricRow = ({ metric, requestRemove }) => {
  if (metric.loading) {
    return <Spinner />;
  }
  return (
    <div className="border-bottom py-3">
      <div className="row">
        <div className="col">
          <Typography>{metric.name}</Typography>
        </div>
        <div className="col-md-3">
          <FontAwesomeIcon
            icon={faCircleXmark}
            className="cursor-pointer float-right text-danger"
            style={{ fontSize: "1.5em" }}
            onClick={() => requestRemove(metric)}
          />
        </div>
      </div>
    </div>
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
  const commonIds = learningMetrics
    ? learningMetrics?.map((_) => _.commonId)
    : [];
  const [newMetricOption, setNewMetricOption] = React.useState();
  const handleUpdateLearningMetrics = () => {
    commonIds.push(newMetricOption.value.commonId);
    setLearningMetrics(commonIds);
  };
  const handleRemove = (metric) => {
    console.debug(metric);
    const newIds = commonIds.filter((_) => _ !== metric.commonId);
    console.debug(newIds);
    setLearningMetrics(newIds);
  };
  return (
    <React.Fragment>
      <BigPopup
        isOpen={isOpen}
        setIsOpen={setIsOpen}
        header="Add a Learning Metric"
        headerDivider
        buttons={
          <button
            className="btn btn-block btn-primary"
            onClick={handleUpdateLearningMetrics}
          >
            Add
          </button>
        }
      >
        {error ? <ErrorCard error={error} /> : null}

        <div className="mt-4" style={{ minHeight: "200px" }}>
          <AsyncSelectMetric onChange={setNewMetricOption} />
        </div>
      </BigPopup>

      {error ? <ErrorCard error={error} /> : null}

      {learningMetrics.map((f) => (
        <LearningMetricRow metric={f} key={f.id} requestRemove={handleRemove} />
      ))}

      {learningMetrics.length === 0 ? (
        <div className="d-flex flex-wrap justify-content-center mt-4 mb-4">
          <div className="mb-2 text-secondary text-center w-100">
            {recommender.targetType === "business"
              ? "Business targets do not support learning metrics"
              : "There are no Learning Metrics yet"}
          </div>
          {recommender.targetType === "business" ? null : (
            <button
              className="btn btn-primary px-4 mt-1"
              onClick={() => setIsOpen(true)}
            >
              Add Learning Metric
            </button>
          )}
        </div>
      ) : null}

      {learningMetrics.length > 0 ? (
        <div className="mt-4">
          <button
            disabled={recommender.targetType === "business"}
            className="btn btn-primary float-right px-4"
            onClick={() => setIsOpen(true)}
          >
            Add Learning Metric
          </button>
        </div>
      ) : null}
    </React.Fragment>
  );
};
