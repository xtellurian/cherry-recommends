import React from "react";
import { NoteBox } from "./NoteBox";

import "./css/recommender-status-box.css";

const label = "Recommender Status";
export const RecommenderStatusBox = ({ recommender }) => {
  if (!recommender.modelRegistration) {
    return (
      <NoteBox cardBodyClassName="training" label={label}>
        Exploring for good recommendations.
      </NoteBox>
    );
  } else {
    return (
      <NoteBox cardBodyClassName="ready" label={label}>
        Discovered good recommendations.
      </NoteBox>
    );
  }
};
