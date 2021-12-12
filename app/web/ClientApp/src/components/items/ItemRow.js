import React from "react";
import { Link } from "react-router-dom";
import { EntityRow } from "../molecules/layout/EntityRow";

export const ItemRow = ({ item, children }) => {
  return (
    <EntityRow>
      <div className="col-4">
        <h5>{item.name}</h5>
      </div>
      <div className="col">
        <p>{item.description}</p>
      </div>

      <div className="col-3 text-right">
        <Link to={`/recommendable-items/detail/${item.id}`}>
          <button className="btn btn-outline-primary">View</button>
        </Link>
        {children}
      </div>
    </EntityRow>
  );
};
