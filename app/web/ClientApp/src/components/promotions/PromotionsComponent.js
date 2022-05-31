import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";

import { RecommendableItemsSummary } from "./PromotionsSummary";
import { CreateItem } from "./CreatePromotion";
import { ItemDetail } from "./PromotionDetail";

const DefaultComponent = () => {
  return (
    <Routes>
      <Route index element={<RecommendableItemsSummary />} />
      <Route path="detail/:id" element={<ItemDetail />} />
      <Route path="create" element={<CreateItem />} />
    </Routes>
  );
};

const PromotionsComponent = () => {
  return (
    <Routes>
      <Route path="promotions/*" element={<DefaultComponent />} />

      <Route index element={<Navigate to="promotions" />} />
    </Routes>
  );
};

export default PromotionsComponent;
