import React from "react";
import { Routes, Route } from "react-router-dom";

import { CreateSegment } from "./CreateSegment";
import { SegmentSummary } from "./SegmentSummary";
import { SegmentDetail } from "./SegmentDetail";
import { CreateButtonClassic } from "../molecules/CreateButton";
import { useFeatureFlag } from "../launch-darkly/hooks";

const SegmentsHome = () => {
  return (
    <div>
      <CreateButtonClassic to="/segments/create" className="float-right">
        Create New Segment
      </CreateButtonClassic>
      <SegmentSummary />
    </div>
  );
};

export const SegmentsComponent = () => {
  const flag = useFeatureFlag("segments", true);

  if (!flag) {
    return null;
  }

  return (
    <Routes>
      <Route index element={<SegmentsHome />} />
      <Route path="detail/:id" element={<SegmentDetail />} />
      <Route path="create" element={<CreateSegment />} />
    </Routes>
  );
};
