import React from "react";
import { Link } from "react-router-dom";
import { EntityRow } from "./layout/EntityRow";

export const BusinessListItem = ({ business }) => {
  return (
    <EntityRow>
      <div className="col">
        <h5>{business.name || business.commonId || business.id}</h5>
      </div>
      <div className="col-3">
        <Link to={`/businesses`}> 
          <button className="btn btn-outline-primary btn-block">Details</button>
        </Link>
      </div>
    </EntityRow>
  );
};

