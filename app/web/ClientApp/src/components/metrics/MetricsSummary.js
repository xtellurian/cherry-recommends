import React from "react";

import { useMetrics } from "../../api-hooks/metricsApi";

import { CreateButtonClassic } from "../molecules/CreateButton";
import { MetricRow } from "./MetricRow";
import { EmptyList, Paginator, Title, ErrorCard, Spinner } from "../molecules";

import { metricsHash } from "../menu/MenuIA";

const MetricsSummary = () => {
  const metrics = useMetrics();
  return (
    <React.Fragment>
      <CreateButtonClassic
        className="float-right"
        to={{
          pathname: "/metrics/create",
          hash: metricsHash.create,
        }}
      >
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
