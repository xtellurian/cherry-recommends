import React from "react";
import { Routes, Route } from "react-router-dom";

import { BusinessesSummary } from "./BusinessesSummary";
import { BusinessDetail } from "./BusinessDetail";
import { CreateBusiness } from "./CreateBusiness";
import { EditBusinessProperties } from "./EditBusinessProperties";
import { BusinessMetrics } from "./BusinessMetrics";
import { CreateEvent } from "./CreateEvent";

export const BusinessesComponent = () => {
  return (
    <Routes>
      <Route index element={<BusinessesSummary />} />
      <Route path="detail/:id" element={<BusinessDetail />} />
      <Route path="create" element={<CreateBusiness />} />
      <Route path="edit-properties/:id" element={<EditBusinessProperties />} />
      <Route path="metrics/:id" element={<BusinessMetrics />} />
      <Route path="create-event/:id" element={<CreateEvent />} />
    </Routes>
  );
};
