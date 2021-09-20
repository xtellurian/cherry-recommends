import React from "react";
import { Circle, CircleFill } from "react-bootstrap-icons";

export const ActiveIndicator = ({ isActive, children }) => {
  return (
    <React.Fragment>
      <div className="row">
        <div className="col">{children}</div>
        <div className="col-1">
          {isActive ? (
            <CircleFill className="mr-3 text-success" />
          ) : (
            <Circle className="mr-3" />
          )}
        </div>
      </div>
    </React.Fragment>
  );
};
