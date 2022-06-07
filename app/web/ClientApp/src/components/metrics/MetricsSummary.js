import React from "react";

import { useMetrics } from "../../api-hooks/metricsApi";

import { MetricRow } from "./MetricRow";
import { EmptyList, Paginator, Spinner } from "../molecules";
import Layout, {
  CreateEntityButton,
} from "../molecules/layout/EntitySummaryLayout";

const MetricsSummary = () => {
  const metrics = useMetrics();
  return (
    <Layout
      header="Metrics"
      createButton={
        <CreateEntityButton to="/metrics/metrics/create">
          Create Metric
        </CreateEntityButton>
      }
      error={metrics.error}
    >
      {metrics.loading && <Spinner />}
      {metrics.items && metrics.items.length === 0 && (
        <EmptyList>There are no metrics.</EmptyList>
      )}
      {metrics.items &&
        metrics.items.map((f) => <MetricRow key={f.id} metric={f} />)}

      {metrics.pagination && <Paginator {...metrics.pagination} />}
    </Layout>
  );
};

export default MetricsSummary;
