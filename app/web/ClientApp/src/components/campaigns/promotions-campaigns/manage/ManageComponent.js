import React from "react";
import { Routes, Route } from "react-router-dom";

import Weights from "./ManageWeights";
import { ManageItems } from "./ManagePromotions";

export const ManageComponent = () => {
  return (
    <Routes>
      <Route path="promotions/:id" element={<ManageItems />} />
      <Route path="weights/:id" element={<Weights />} />
    </Routes>
  );
};
