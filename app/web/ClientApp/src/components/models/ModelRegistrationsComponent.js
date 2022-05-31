import React from "react";
import { Routes, Route } from "react-router-dom";

import AuthorizeRoute from "../auth0/ProtectedRoute";
import { CreateModelRegistration } from "./CreateModelRegistration";
import { ModelRegistrationsSummary } from "./ModelRegistrationsSummary";
import { TestModel } from "./TestModel";

export const ModelRegistrationsComponent = () => {
  return (
    <Routes>
      <Route
        index
        element={<AuthorizeRoute component={ModelRegistrationsSummary} />}
      />
      <Route
        path="create"
        element={<AuthorizeRoute component={CreateModelRegistration} />}
      />
      <Route
        path="test/:id"
        element={<AuthorizeRoute component={TestModel} />}
      />
    </Routes>
  );
};
