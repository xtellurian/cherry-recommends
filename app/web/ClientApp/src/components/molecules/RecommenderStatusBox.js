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
  } else if (recommender.useOptimiser) {
    return (
      <div className="float-right text-muted">Using Internal Optimiser</div>
    );
  } else if (recommender.modelRegistrationId) {
    return <div className="float-right text-muted">Using Custom Model</div>;
  } else
    return <div className="float-right text-muted">Using Random Choices</div>;
};
