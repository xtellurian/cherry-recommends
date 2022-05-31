import React from "react";
import { Route, Navigate, Routes } from "react-router-dom";

import MetricsSummary from "./MetricsSummary";
import Create from "./CreateMetric";
import Detail from "./MetricDetail";
import SetMetricValue from "./SetMetricValue";
import GeneratorsComponent from "./metric-generators/MetricGeneratorsComponent";
import ActivityFeed from "./ActivityFeed";

const DefaultComponent = () => {
  return (
    <Routes>
      <Route index element={<MetricsSummary />} />
      <Route path="create" element={<Create />} />
      <Route path="detail/:id" element={<Detail />} />
      <Route path="set-value/:id" element={<SetMetricValue />} />
      <Route path="generators/*" element={<GeneratorsComponent />} />
    </Routes>
  );
};

const MetricsComponent = () => {
  return (
    <Routes>
      <Route path="metrics/*" element={<DefaultComponent />} />
      <Route path="activity-feed" element={<ActivityFeed />} />

      <Route index element={<Navigate to="metrics" />} />
    </Routes>
  );
};

export default MetricsComponent;
