import React from "react";
import { Route, Routes } from "react-router-dom";

import GeneratorsSummary from "./MetricGeneratorsSummary";

const MetricGeneratorsComponent = () => {
  return (
    <Routes>
      <Route index element={<GeneratorsSummary />} />
    </Routes>
  );
};

export default MetricGeneratorsComponent;
