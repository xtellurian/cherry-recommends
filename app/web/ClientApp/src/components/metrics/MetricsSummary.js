import React from "react";
import { EmptyList, Paginator, Title, ErrorCard, Spinner } from "../molecules";
import { ClickableRow } from "../molecules/layout/ClickableRow";

import { useMetrics } from "../../api-hooks/metricsApi";
import { CreateButtonClassic } from "../molecules/CreateButton";

const MetricRow = ({ metric }) => {
  return (
    <ClickableRow
      buttonText="Detail"
      label={metric.name}
      to={`/metrics/detail/${metric.id}`}
    />
  );
};

const MetricsSummary = () => {
  const metrics = useMetrics();
  return (
    <React.Fragment>
      <CreateButtonClassic className="float-right" to="/metrics/create">
        Create Metric
      </CreateButtonClassic>
      <Title>Metrics</Title>
      <hr />
      {metrics.loading && <Spinner />}
      {metrics.items && metrics.items.length === 0 && (
        <EmptyList>There are no metrics.</EmptyList>
      )}
      {metrics.error && <ErrorCard error={metrics.error} />}
      {metrics.items &&
        metrics.items.map((f) => <MetricRow key={f.id} metric={f} />)}

      {metrics.pagination && <Paginator {...metrics.pagination} />}
    </React.Fragment>
  );
};

export default MetricsSummary;