import React from "react";
import { NoteBox } from "./NoteBox";

import "./css/recommender-status-box.css";

const label = "Recommender Status";
export const RecommenderStatusBox = ({ recommender }) => {
  if (!recommender.modelRegistrationId) {
    return (
      <NoteBox cardBodyClassName="training" label={label}>
        Exploring for good recommendations.
      </NoteBox>
    );
  } else if (
    recommender.modelRegistration &&
    recommender.modelRegistration.hostingType === "azureFunctions"
  ) {
    return (
      <NoteBox cardBodyClassName="ready" label={label}>
        Using Cherry Auto-AI
      </NoteBox>
    );
  } else
    return (
      <NoteBox cardBodyClassName="ready" label={label}>
        Discovered good recommendations.
      </NoteBox>
    );
};
