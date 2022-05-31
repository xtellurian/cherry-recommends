import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";

import { UploadTrackedUserComponent } from "./UploadUsers";
import { CustomersSummary } from "./CustomersSummary";
import { CustomerDetail } from "./CustomerDetail";
import { CreateCustomer } from "./CreateCustomer";
import { LinkToIntegratedSystem } from "./LinkToIntegratedSystem";
import { EditProperties } from "./EditProperties";
import { CreateEvent } from "./CreateEvent";
import Metrics from "./Metrics";
import { BusinessesComponent } from "../businesses/BusinessesComponent";
import { SegmentsComponent } from "../segments/SegmentsComponent";
import { DataViewComponent } from "../data/DataViewComponent";

const DefaultComponent = () => {
  return (
    <Routes>
      <Route index element={<CustomersSummary />} />
      <Route path="upload" element={<UploadTrackedUserComponent />} />
      <Route path="create" element={<CreateCustomer />} />
      <Route path="detail/:id" element={<CustomerDetail />} />
      <Route
        path="link-to-integrated-system/:id"
        element={<LinkToIntegratedSystem />}
      />
      <Route path="metrics/:id" element={<Metrics />} />
      <Route path="edit-properties/:id" element={<EditProperties />} />
      <Route path="create-event/:id" element={<CreateEvent />} />
    </Routes>
  );
};

export const CustomersComponent = () => {
  return (
    <Routes>
      <Route path="customers/*" element={<DefaultComponent />} />
      <Route path="businesses/*" element={<BusinessesComponent />} />
      <Route path="segments/*" element={<SegmentsComponent />} />
      <Route path="dataview/*" element={<DataViewComponent />} />

      <Route index element={<Navigate to="customers" />} />
    </Routes>
  );
};
