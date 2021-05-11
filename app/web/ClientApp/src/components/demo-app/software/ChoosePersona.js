import React from "react";
import { Link } from "react-router-dom";

const GoToCancelButton = ({ persona, children }) => {
  return (
    <Link to={`/demo/software/landing?persona=${persona}`}>
      <button className="btn btn-primary btn-block m-2">{children}</button>
    </Link>
  );
};

const bottomStyle = {
  position: "absolute",
  left: 0,
  bottom: 0,
  right: "vw",
  marginRight: "20px",
  marginBottom: "20px",
};

export const ChoosePersona = () => {
  return (
    <React.Fragment>
      <div className="row">
        <div className="col">
          <div className="text-center">
            <h5>Choose your persona</h5>
            <hr />
          </div>
          <div className="text-center">
            <GoToCancelButton persona="new">
              I've recently signed up
            </GoToCancelButton>
            <GoToCancelButton persona="infrequent">
              I'm an infrequent user
            </GoToCancelButton>
            <GoToCancelButton persona="power">
              I'm a power user
            </GoToCancelButton>
          </div>
        </div>
        <div className="col-3">
          <div className="text-center mt-5">
            Before working with us, one of our customers offered 50% to anyone
            who was about to cancel.
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};
