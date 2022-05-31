import React from "react";
import { Routes, Route } from "react-router-dom";

import { TenantsComponent } from "./components/tenants/TenantsComponent";

import "./global-css/cherry.css";

const ManagementApp = () => {
  return (
    <Routes>
      <Route index path="*" element={<TenantsComponent />} />
    </Routes>
  );
};

export default ManagementApp;
