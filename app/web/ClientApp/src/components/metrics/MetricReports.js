import React from "react";

import { AggregateMetricChartLoader } from "../molecules/charts/AggregateMetricChartLoader";
import { MetricHistogramLoader } from "../molecules/charts/MetricHistogramLoader";
import { BigPopup } from "../molecules/popups/BigPopup";
import { DateTimeField } from "../molecules/DateTimeField";
import { useAggregateMetricsNumeric } from "../../api-hooks/metricsApi";
import {
  MetricQuartilesChartLoader,
  MetricQuartilesThumbnailChartLoader,
} from "../molecules/charts/MetricQuartilesChartLoader";
import { Spinner } from "../molecules";

const ReportCard = ({ name, lastUpdated, chart, chartThumbnail }) => {
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
        <span className="text-center text-black-50 mt-1">
          <DateTimeField label="Last updated" date={lastUpdated} />
        </span>
        {chartThumbnail}
        <button
          onClick={() => setOpenReport(true)}
          className="btn btn-primary mx-auto mt-4 w-50"
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

const FigureCard = ({ name, label, value, loading }) => {
  return (
    <div className="card py-5 px-3 h-100 d-flex justify-content-center align-items-center">
      {loading ? (
        <Spinner />
      ) : (
        <React.Fragment>
          <span
            style={{ fontSize: "2.9375rem", color: "var(--cherry-dark-pink)" }}
          >
            {value ?? "--"}
          </span>
          <span className="text-center text-capitalize">{name}</span>
          <span className="font-weight-bold text-center text-capitalize">
            {label}
          </span>
        </React.Fragment>
      )}
    </div>
  );
};

const MetricReports = ({ metric }) => {
  const aggregateMetricsNumeric = useAggregateMetricsNumeric({ id: metric.id });

  const weeklyMeanNumericValue =
    aggregateMetricsNumeric?.length &&
    aggregateMetricsNumeric[0]?.weeklyMeanNumericValue;

  const weeklyDistinctCustomerCount =
    aggregateMetricsNumeric?.length &&
    aggregateMetricsNumeric[0]?.weeklyDistinctCustomerCount;

  return (
    <React.Fragment>
      <div className="mt-3">
        <div className="row">
          <div className="col-3 mt-2">
            <div className="row h-100">
              <div className="col-12">
                <FigureCard
                  name={metric?.name}
                  label="Mean Value"
                  value={
                    weeklyMeanNumericValue && weeklyMeanNumericValue % 1 !== 0
                      ? weeklyMeanNumericValue.toFixed(2)
                      : weeklyMeanNumericValue
                  }
                  loading={
                    aggregateMetricsNumeric && aggregateMetricsNumeric.loading
                  }
                />
              </div>
              <div className="col-12 mt-4">
                <FigureCard
                  name={metric?.name}
                  label="Customer Count"
                  value={weeklyDistinctCustomerCount}
                  loading={
                    aggregateMetricsNumeric && aggregateMetricsNumeric.loading
                  }
                />
              </div>
            </div>
          </div>
          <div className="col-4 mt-2">
            <div className="row h-100">
              <div className="col-12">
                <ReportCard
                  name={`Mean Weekly - ${metric.name}`}
                  lastUpdated={metric.lastUpdated}
                  chart={<AggregateMetricChartLoader metric={metric} />}
                />
              </div>
              <div className="col-12 mt-4">
                <ReportCard
                  name={`Distribution - ${metric.name}`}
                  lastUpdated={metric.lastUpdated}
                  chart={<MetricHistogramLoader metric={metric} />}
                />
              </div>
            </div>
          </div>
          <div className="col-5 mt-2">
            <ReportCard
              name={`Quartiles - ${metric.name}`}
              lastUpdated={metric.lastUpdated}
              chartThumbnail={
                <MetricQuartilesThumbnailChartLoader metric={metric} />
              }
              chart={<MetricQuartilesChartLoader metric={metric} />}
            />
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

export default MetricReports;
