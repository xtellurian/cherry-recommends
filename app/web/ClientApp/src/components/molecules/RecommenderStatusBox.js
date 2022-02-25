import React from "react";
import { NoteBox } from "./NoteBox";

import "./css/recommender-status-box.css";
import { Spinner } from ".";

const label = "Recommender Status";
export const RecommenderStatusBox = ({ className, recommender }) => {
  if (recommender.loading) {
    return (
      <div className="float-right">
        <Spinner />
      </div>
    );
  } else if (!recommender.modelRegistrationId) {
    return (
      <NoteBox className={className} cardBodyClassName="training" label={label}>
        Exploring for good recommendations.
      </NoteBox>
    );
  } else if (
    recommender.modelRegistration &&
    recommender.modelRegistration.hostingType === "azureFunctions"
  ) {
    return (
      <NoteBox className={className} cardBodyClassName="ready" label={label}>
        Using Cherry Auto-AI
      </NoteBox>
    );
  } else
    return (
      <NoteBox className={className} cardBodyClassName="ready" label={label}>
        Discovered good recommendations.
      </NoteBox>
    );
};
