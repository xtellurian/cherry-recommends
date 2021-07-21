import React from "react";
import { NoteBox } from "./NoteBox";

import "./css/recommender-status-box.css"

export const RecommenderStatusBox = ({ recommender }) => {
  if (!recommender.modelRegistration) {
    return (
      <NoteBox cardBodyClassName="training" label="Recommender Status">
        Searching for optimum recommendations.
      </NoteBox>
    );
  } else {

      return <NoteBox cardBodyClassName="ready" label="Status">Exploiting prior knowledge.</NoteBox>;
  }
};
