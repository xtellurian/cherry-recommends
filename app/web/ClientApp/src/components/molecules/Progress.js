import React from "react";
import { useInterval } from "../../utility/useInterval";

const FRAMES_PER_SECOND = 12;
const MAX_PROGRESS = 100;
const sanitiseProgress = (progress) => {
  return Math.round(progress);
};
export const Progress = ({ children, seconds }) => {
  const increment = MAX_PROGRESS / ((seconds || 5) * FRAMES_PER_SECOND);
  const [progress, setProgress] = React.useState(1);
  useInterval(
    () => setProgress(Math.min(progress + increment, MAX_PROGRESS)),
    1000 / FRAMES_PER_SECOND
  );

  return (
    <div className="justify-content-center">
      <div className="progress">
        <div
          className="progress-bar progress-bar-striped progress-bar-animated bg-info"
          role="progressbar"
          aria-valuenow="50"
          aria-valuemin="0"
          aria-valuemax="100"
          style={{ width: `${sanitiseProgress(progress)}%` }}
        ></div>
      </div>
      <div className="d-flex justify-content-between">
        <div className="text-left text-muted">{`${sanitiseProgress(
          progress
        )}%`}</div>
        <div className="text-right text-muted">{children}</div>
      </div>
    </div>
  );
};
