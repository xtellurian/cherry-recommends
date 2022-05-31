import React from "react";
import { Routes, Route } from "react-router-dom";

import { CreateEnvironment } from "./CreateEnvironment";
import { EnvironmentsSummary } from "./EnvironmentsSummary";

export const EnvironmentsComponent = () => {
  return (
    <Routes>
      <Route index element={<EnvironmentsSummary />} />
      <Route path="create" element={<CreateEnvironment />} />
      {/* <Route path="reward-selector/:id" element={<RewardSelectorDetail />} /> */}
    </Routes>
  );
};
