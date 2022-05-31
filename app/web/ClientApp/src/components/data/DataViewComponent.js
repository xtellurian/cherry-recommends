import React from "react";
import { Routes, Route } from "react-router-dom";

import { Title } from "../molecules/layout";
import { ViewEventData } from "./ViewEventData";

const DataViewHome = () => {
  return (
    <div>
      <Title>Events Overview</Title>
      <hr />
      <ViewEventData />
    </div>
  );
};

export const DataViewComponent = () => {
  return (
    <Routes>
      <Route index element={<DataViewHome />} />
      <Route path="events" element={<ViewEventData />} />
    </Routes>
  );
};
