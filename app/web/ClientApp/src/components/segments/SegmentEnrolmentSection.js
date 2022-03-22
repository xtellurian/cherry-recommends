import React, { useState } from "react";

import {
  AddSegmentEnrolmentRule,
  SegmentEnrolmentRuleList,
} from "./SegmentEnrolment";

export const SegmentEnrolmentSection = ({ id }) => {
  const [showEnrolmentRuleForm, setShowEnrolmentRuleForm] = useState(false);

  const toggleEnrolmentRuleForm = () => {
    setShowEnrolmentRuleForm(
      (oldShowEnrolmentRuleForm) => !oldShowEnrolmentRuleForm
    );
  };

  return showEnrolmentRuleForm ? (
    <AddSegmentEnrolmentRule id={id} onBack={toggleEnrolmentRuleForm} />
  ) : (
    <SegmentEnrolmentRuleList id={id} onAdd={toggleEnrolmentRuleForm} />
  );
};
