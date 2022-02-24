import React from "react";

import { AggregateMetricChartLoader } from "../molecules/charts/AggregateMetricChartLoader";
import { MetricHistogramLoader } from "../molecules/charts/MetricHistogramLoader";
import { BigPopup } from "../molecules/popups/BigPopup";
import { DateTimeField } from "../molecules/DateTimeField";

const ReportCard = ({ name, lastUpdated, chart }) => {
  const [openReport, setOpenReport] = React.useState(false);

  return (
    <React.Fragment>
      <div className="card py-5 px-3 h-100 d-flex justify-content-center align-items-center">
        <span
          className="font-weight-bold text-center text-capitalize"
          style={{ fontSize: "1.25rem" }}
        >
          {name}
        </span>
        <span
          className="text-center text-secondary mt-1"
          style={{ fontSize: "0.9rem" }}
        >
          <DateTimeField label="Last updated" date={lastUpdated} />
        </span>
        <button
          onClick={() => setOpenReport(true)}
          className="btn btn-primary mx-auto mt-4"
          style={{ minWidth: "128px", maxWidth: "50%" }}
        >
          Show report
        </button>
        <BigPopup isOpen={openReport} setIsOpen={setOpenReport}>
          {chart}
        </BigPopup>
      </div>
    </React.Fragment>
  );
};

const MetricReports = ({ metric }) => {
  return (
    <React.Fragment>
      <div className="mt-3">
        <div className="row">
          <div className="col-4 mt-2">
            <ReportCard
              name={`Mean Weekly - ${metric.name}`}
              lastUpdated={metric.lastUpdated}
              chart={<AggregateMetricChartLoader metric={metric} />}
            />
          </div>
          <div className="col-4 mt-2">
            <ReportCard
              name={`Distribution - ${metric.name}`}
              lastUpdated={metric.lastUpdated}
              chart={<MetricHistogramLoader metric={metric} />}
            />
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

export default MetricReports;
