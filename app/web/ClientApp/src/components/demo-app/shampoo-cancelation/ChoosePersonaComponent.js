import React from "react";
import { Link } from "react-router-dom";

const GoToCancelButton = ({ persona, children }) => {
  return (
    <Link to={`/demo/shampoo/cancel?persona=${persona}`}>
      <button className="btn btn-primary btn-block">{children}</button>
    </Link>
  );
};
export const ChoosePersonaComponent = () => {
  return (
    <React.Fragment>
      <div>
        <h2>Choose your persona</h2>
        <hr />
      </div>
      <div className="row">
        <div className="col">
          <GoToCancelButton persona="new">I'm a new customer</GoToCancelButton>
        </div>
        <div className="col">
          <GoToCancelButton persona="normal">
            I'm a normal customer
          </GoToCancelButton>
        </div>
        <div className="col">
          <GoToCancelButton persona="loyal">
            I'm a loyal customer
          </GoToCancelButton>
        </div>
      </div>
    </React.Fragment>
  );
};
