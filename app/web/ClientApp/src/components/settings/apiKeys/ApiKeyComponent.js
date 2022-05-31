import React from "react";
import { Routes, Route } from "react-router-dom";

import { ListApiKeys } from "./ListApiKeys";
import { CreateApiKey } from "./CreateApiKey";

export const ApiKeyComponent = () => {
  return (
    <Routes>
      <Route index element={<ListApiKeys />} />
      <Route path="create" element={<CreateApiKey />} />
    </Routes>
  );
};
