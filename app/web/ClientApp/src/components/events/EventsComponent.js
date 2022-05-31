import React from "react";
import { Routes, Route } from "react-router-dom";

import { EventDetailPage } from "./EventDetail";

export const EventsComponent = () => {
  return (
    <Routes>
      <Route path="detail/:id" element={<EventDetailPage />} />
    </Routes>
  );
};
