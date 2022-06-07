import React from "react";
import { Routes, Route } from "react-router-dom";

import { CreateSegment } from "./CreateSegment";
import { SegmentSummary } from "./SegmentSummary";
import { SegmentDetail } from "./SegmentDetail";
import { useFeatureFlag } from "../launch-darkly/hooks";

export const SegmentsComponent = () => {
  const flag = useFeatureFlag("segments", true);

  if (!flag) {
    return null;
  }

  return (
    <Routes>
      <Route index element={<SegmentSummary />} />
      <Route path="detail/:id" element={<SegmentDetail />} />
      <Route path="create" element={<CreateSegment />} />
    </Routes>
  );
};
