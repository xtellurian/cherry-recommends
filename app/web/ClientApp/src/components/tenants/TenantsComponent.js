import React from "react";
import { Routes, Route } from "react-router-dom";

import { ManagementPage } from "./managementPage";
import { CreateTenantSection } from "./CreateTenantSection";

export const TenantsComponent = () => {
  return (
    <Routes>
      <Route index element={<ManagementPage />} />
      <Route path="create-tenant" element={<CreateTenantSection />} />
    </Routes>
  );
};
