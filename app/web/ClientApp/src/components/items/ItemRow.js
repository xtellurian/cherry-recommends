import React from "react";
import { Link } from "react-router-dom";
import { EntityRow } from "../molecules/layout/EntityRow";

export const ItemRow = ({ item, children }) => {
  return (
    <EntityRow>
      <div className="col-sm">
        <h5>{item.name}</h5>
      </div>
      <div className="col-lg-6">
        <p>{item.description}</p>
      </div>

      <div className="col-sm text-right">
        <Link to={`/recommendable-items/detail/${item.id}`}>
          <button className="btn btn-outline-primary m-auto">View</button>
        </Link>
        {children}
      </div>
    </EntityRow>
  );
};
