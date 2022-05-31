import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";

import { ParametersSummary } from "./ParametersSummary";
import { CreateParameter } from "./CreateParameter";

const DefaultComponent = () => {
  return (
    <Routes>
      <Route index element={<ParametersSummary />} />
      <Route path="create" element={<CreateParameter />} />
    </Routes>
  );
};

export const ParametersComponent = () => {
  return (
    <Routes>
      <Route path="parameters/*" element={<DefaultComponent />} />

      <Route index element={<Navigate to="parameters" />} />
    </Routes>
  );
};
