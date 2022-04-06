import React from "react";
import { Navigation } from "./Navigation";
import { Spinner } from "./Spinner";

export const EntityField = ({ label, entity, to }) => {
  return (
    <div className="input-group mb-3">
      <div className="input-group-prepend ml-1">
        <span className="input-group-text">{label}</span>
      </div>
      {entity.loading ? (
        <Spinner />
      ) : (
        <React.Fragment>
          <input
            type="text"
            value={entity.name || entity.commonId || entity.id}
            className="form-control"
            aria-label={label}
            disabled
          />
          <Navigation to={to}>
            <button className="btn btn-outline-secondary" type="button">
              View
            </button>
          </Navigation>
        </React.Fragment>
      )}
    </div>
  );
};
