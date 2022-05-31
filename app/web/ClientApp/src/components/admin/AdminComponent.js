import React from "react";
import { Routes, Route } from "react-router-dom";

import { ModelRegistrationsComponent } from "../models/ModelRegistrationsComponent";

export const AdminComponent = () => {
  return (
    <React.Fragment>
      <div className="border border-warning rounded text-muted text-center mb-4">
        Admin Zone
      </div>
      <Routes>
        <Route path="models/*" element={<ModelRegistrationsComponent />} />
      </Routes>
    </React.Fragment>
  );
};
