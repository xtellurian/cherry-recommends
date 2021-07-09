import React from "react";
import { NoteBox } from "./NoteBox";

import "./css/recommender-status-box.css"

export const RecommenderStatusBox = ({ recommender }) => {
  if (!recommender.modelRegistration) {
    return (
      <NoteBox cardBodyClassName="training" label="Status">
        This recommender's model is still in training, but you can still use it.
      </NoteBox>
    );
  } else {

      return <NoteBox cardBodyClassName="ready" label="Status">Ready to Invoke.</NoteBox>;
  }
};
