import React from "react";
import dayjs from "dayjs";

import { AggregateMetricChartLoader } from "../molecules/charts/AggregateMetricChartLoader";
import { BigPopup } from "../molecules/popups/BigPopup";

const ReportCard = ({ name, lastUpdated, chart }) => {
  const [openReport, setOpenReport] = React.useState(false);

  return (
    <React.Fragment>
      <div className="card py-5 px-3">
        <span
          className="font-weight-bold text-center"
          style={{ fontSize: "1.25rem" }}
        >
          {name}
        </span>
        <span className="text-center mt-1">
          Last updated: {dayjs(lastUpdated).format("DD/MM/YYYY, hh:mm:ss a")}
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
        <div className="row no-gutters">
          <div className="col-4 mt-2">
            <ReportCard
              name={`Mean Weekly - ${metric.name}`}
              subheader={metric.lastUpdated}
              chart={<AggregateMetricChartLoader metric={metric} />}
            />
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

export default MetricReports;
