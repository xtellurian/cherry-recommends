import React from "react";
import { Link } from "react-router-dom";

export const EntityField = ({ label, entity, to }) => {
  return (
    <div className="input-group mb-3">
      <div className="input-group-prepend ml-1">
        <span className="input-group-text">{label}</span>
      </div>
      <input
        type="text"
        value={entity.name || entity.commonId || entity.id}
        className="form-control"
        aria-label={label}
        disabled
      />
      <Link to={to}>
        <button className="btn btn-outline-secondary" type="button">
          View
        </button>
      </Link>
    </div>
  );
};
