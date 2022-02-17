import React from "react";
import { AggregateMetricChartLoader } from "../molecules/charts/AggregateMetricChartLoader";
import { BigPopup } from "../molecules/popups/BigPopup";

const MetricReports = ({ metric }) => {
  const [aggregateMetricsChartOpen, setAggregateMetricsChartOpen] =
    React.useState(false);

  return (
    <React.Fragment>
      <BigPopup
        isOpen={aggregateMetricsChartOpen}
        setIsOpen={setAggregateMetricsChartOpen}
      >
        <AggregateMetricChartLoader metric={metric} />
      </BigPopup>
      <div
        className="d-flex justify-content-center align-items-center"
        style={{ minHeight: "50vh" }}
      >
        <div>
          <p>Mean weekly report for {metric.name}</p>
          <div className="mt-4 text-center">
            <button
              onClick={() => setAggregateMetricsChartOpen(true)}
              className="btn btn-outline-primary"
              style={{ minWidth: "128px" }}
            >
              Show report
            </button>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

export default MetricReports;
